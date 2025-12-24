Here is the comprehensive **Technical Roadmap** for **Feather Quest**.

**DevBridge Analysis:**
The decision to start in **Godot .NET** and migrate to **Unity** later is a critical architectural constraint. If we write standard "Godot Script" logic (putting game logic directly into Node classes), your migration in 6 months will be a total rewrite.

To avoid this, we will use the **Model-View-Presenter (MVP)** pattern.
*   **The Logic (Model):** Pure C# classes (POCOs) with **zero** dependency on Godot or Unity libraries.
*   **The Interface (View):** The Godot Nodes (and later Unity MonoBehaviours) that simply display data and capture input.
*   **The Connector (Presenter):** Scripts that bridge the two.

This `ROADMAP.md` is structured to force the AI Agent to write portable code.

***

# ROADMAP.md

## 1. Technical Synopsis

**Project Name:** Feather Quest: My Wild Year
**Phase:** MVP (Minimum Viable Product)
**Current Engine:** Godot 4.x (.NET Edition)
**Target Platform:** WebGL (HTML5) - Mobile & Desktop Responsive
**Future Path:** Migration to Unity 3D (Month 6+)

### The "Migration-First" Strategy
To ensure the code survives the transition from Godot to Unity, we strictly enforce **Dependency Injection (DI)** and **Interface-Based Architecture**.
*   **Core Logic:** Written in pure C# (Standard 2.1). No `using Godot;` allowed in data models or calculation logic.
*   **Abstractions:** We define interfaces like `IAudioManager` or `IInputProvider`.
*   **Implementations:**
    *   Now: `GodotAudio : IAudioManager`
    *   Later: `UnityAudio : IAudioManager`

### Core Loop Logic (Pseudo-Code)
```csharp
// The Game Loop is engine-agnostic
void Update() {
    switch (CurrentState) {
        case GameState.Exploration:
            if (Input.ClickDetected && Object is BirdCue) {
                TransitionTo(GameState.Encounter);
            }
            break;
        case GameState.Encounter:
            FocusMeter += CalculateFocus(ReticlePosition, BirdVelocity);
            if (FocusMeter >= 100) TransitionTo(GameState.Identification);
            if (PatienceMeter <= 0) BirdFlushes();
            break;
        // ...
    }
}
```

---

## 2. System Architecture (The Blueprints)

### Folder Structure (Strict Enforcement)
*   `/Core` (Pure C# - **NO GODOT CODE HERE**)
    *   `/Models` (BirdData, PlayerProfile, Inventory)
    *   `/Services` (Interfaces for Audio, SaveSystem, API)
    *   `/Logic` (EncounterCalculator, ExpSystem)
*   `/GodotClient` (The Game Engine Project)
    *   `/Scenes`
    *   `/Scripts` (The "View" layer that inherits `Node` and calls `/Core`)
    *   `/Assets`

### Key Systems
1.  **GameDirector (Singleton/Stateless):** Manages the state flow (Exploration $\to$ Encounter $\to$ Summary).
2.  **BirdDatabase (JSON Repo):** A `Dictionary<string, BirdDef>` for O(1) retrieval with **1-to-many variants per bird**.
3.  **EncounterEngine (Math):** Calculates the "Sway" and "Focus" math and produces **PhotoQuality** scoring.
    *   *Focus Formula:* `FocusGain = (1.0 - Distance(Reticle, Target)) * LensMultiplier * DeltaTime`
    *   *Photo Formula:* `quality = Clamp01((1 - DistanceFromCenter) * stabilityAvg)`
4.  **WorldStateManager:** Publishes `TimeOfDay` and `Weather` context for spawn weighting and difficulty scaling.
5.  **IntegrationManager:** Handles HTTP requests to eBird and LLM (Gemma).

### Data Structures
*   **BirdDefinition:**
    ```csharp
    public class BirdDefinition {
        public string ID; // "btwarbler_01"
        public string CommonName;
        public string ScientificName;
        public DifficultyTier Tier;
        public string[] FieldMarks; // ["blue_wings", "black_throat"]
        public List<PlumageVariant> Variants; // seasonal/sex plumage set
        public List<BirdCall> Calls; // song, call, alarm, etc.
    }

    public class PlumageVariant {
        public Season Season; // Breeding, NonBreeding, Molting
        public Gender Gender; // Male, Female, Unknown
        public AssetReference SpritePath;
        public DifficultyTier DifficultyRating;
    }

    public class BirdCall {
        public CallType Type; // Song, Call, Alarm
        public AssetReference AudioPath;
        public AssetReference SpectrogramPath; // pre-baked image
    }
    ```

---

## 3. Risk Assessment

1.  **Audio Analysis (Spectrograms):**
    *   *Risk:* Generating real-time spectrograms in Godot WebGL via C# is performance-heavy and complex.
    *   *Mitigation:* For MVP, use **Pre-baked Images**. Store a `.png` of the spectrogram alongside the `.mp3`. Don't try to generate it at runtime yet.
2.  **Web Export Size:**
    *   *Risk:* High-res bird photography will bloat the download size.
    *   *Mitigation:* Use strict compression (WebP) and implement "Lazy Loading" (load assets only when the Biome is entered).
3.  **Godot C# Web Support:**
    *   *Risk:* Godot .NET Web export is improving but can have issues with certain reflection-heavy libraries (like standard JSON serializers).
    *   *Mitigation:* Use `Newtonsoft.Json` or `System.Text.Json` cautiously; test builds early and often.
4.  **Edge Computer Vision vs Math-Based Approach:**
    *   *Risk:* Running TensorFlow.js or similar CV in-browser will inflate download size and hurt WebGL performance.
    *   *Mitigation:* MVP explicitly replaces CV with math-based scoring (`CalculatePhotoQuality`) and pre-baked spectrograms. Revisit CV only in Phase 2.

---

## 4. Development Backlog (Sprint Plan)

### Sprint 0: Project Scaffold & The "Pure" Core
*Goal: Establish the C# logic that will eventually move to Unity.*

*   **[CORE-001] Repository Setup & Solution Structure**
    *   **User Story:** As a developer, I want a split solution so I don't accidentally mix Engine code with Game Logic.
    *   **Implementation:** Create a Visual Studio Solution. Project A: `FeatherQuest.Core` (Class Lib). Project B: `FeatherQuest.Godot` (Godot App). Reference A in B.

*   **[CORE-002] Bird Database Model & JSON Parser**
    *   **User Story:** As a designer, I want to edit bird stats in a JSON file.
    *   **Implementation:** Create `BirdDefinition` with `Variants` (season/sex) and `Calls` (song/call/alarm with spectrogram path). Write a `BirdLoader` that parses a local JSON file into a `Dictionary<string, BirdDefinition>`.
    *   **Acceptance:** Unit test passes where `BirdLoader.Get("robin").Variants` returns the correct sprites and `Calls` returns multiple audio entries without Godot running.

*   **[CORE-003] The Focus Math Engine**
    *   **User Story:** As a player, I want the binoculars to sway.
    *   **Implementation:** Create `FocusCalculator.cs`. Implement a Perlin Noise or Sine Wave function to generate specific X/Y offsets based on a `stability` float.
    *   **Note:** This logic must be pure math, returning a `Vector2`.

*   **[CORE-004] Photo Quality Calculator**
    *   **User Story:** As a player, I want higher rewards for centered, stable shots.
    *   **Implementation:** Add a pure function `CalculatePhotoQuality(Vector2 finalReticlePos, float stabilityAvg)` that returns a scalar 0.0–1.0 quality value used for Bronze/Silver/Gold tiers.
    *   **Acceptance:** Unit tests confirm expected quality values for center vs edge cases.

*   **[CORE-005] Environment Context Models**
    *   **User Story:** As a system, I need to describe world conditions that influence spawns.
    *   **Implementation:** Define enums `TimeOfDayEnum`, `WeatherEnum`, and a `WorldContext` model used by spawn weighting and difficulty scaling (no engine hooks yet).
    *   **Acceptance:** Models serialize/deserialize and can be injected into pure logic services.

### Sprint 1: Godot Implementation (The Viewer)
*Goal: Get visual feedback on screen.*

*   **[GODOT-001] Biome Scene & Parallax**
    *   **User Story:** As a player, I want to scroll through a forest.
    *   **Implementation:** Godot `ParallaxBackground`. Layers: Sky (Static), Distant Trees (Slow), Midground (Normal), Foreground (Fast).
    *   **Input:** Mouse drag / Touch swipe to scroll camera.

*   **[GODOT-002] Bird Spawning System**
    *   **User Story:** As a player, I want to see visual cues.
    *   **Implementation:** `BirdSpawner` node. It polls the `BirdDatabase` and picks a `PlumageVariant` per spawn. Instantiates a generic `BirdCue` scene (particle effect + audio stream) at random intervals.

*   **[LOGIC-001] Behavioral Spawning Logic (Swarm/Hide)**
    *   **User Story:** As a player, I want birds to behave realistically (swarming after rain, hiding at midday).
    *   **Implementation:** Upgrade `BirdSpawner` to support `SpawnPattern` definitions (Solitary, Flock, Swarm, Hidden). `WorldStateManager` modifies patterns at runtime based on weather/time (e.g., Rain -> Water birds active; Post-Rain -> Robins swarm).
    *   **Acceptance:** Changing weather to "PostRain" triggers swarm behavior for Robins; "Midday" reduces visibility.

*   **[GODOT-003] The Binocular UI (The "Battle")**
    *   **User Story:** As a player, I want to look through binoculars.
    *   **Implementation:** A CanvasLayer with a Binocular Mask (TextureRect). Behind it, the specific Bird Sprite.
    *   **Scripting:** Connect `FocusCalculator` (from Sprint 0) to the Sprite's position. If the player moves the mouse, counteract the sway.

*   **[UI-000] Responsive Layout & Input Testing**
    *   **User Story:** As a player, I want the game to work on my phone and my laptop.
    *   **Implementation:** Create a test plan for different aspect ratios and input methods (Touch vs Mouse). Ensure UI anchors are set correctly in Godot.
    *   **Acceptance:** Game is playable on mobile portrait/landscape and desktop.

### Sprint 2: Game Loop & Identification
*Goal: Complete the "Search > ID > Reward" loop.*

*   **[SYS-001] State Machine Manager**
    *   **User Story:** As a system, I need to know if the player is searching or identifying.
    *   **Implementation:** Implement the FSM. `ExplorationState` enables scrolling. `EncounterState` locks scrolling and enables Binoculars.

*   **[UI-001] ID Interface & Color Wheel**
    *   **User Story:** As a player, I want to select the bird's features.
    *   **Implementation:** Dynamic UI generation.
        *   Create a "Tag Selector" (GridContainer of buttons).
        *   Create logic: `BirdFilter.Filter(selectedTags)` returning a list of matching birds.

*   **[SYS-002] Progression & Save System**
    *   **User Story:** As a player, I want my XP to persist.
    *   **Implementation:** `SaveManager`. Serialize `PlayerProfile` class to local storage (UserDir).
    *   **Data:** `LifeList` (List of strings), `CurrentXP` (int), `Credits` (int).

*   **[SYS-003] World State Manager & Spawn Weighting**
    *   **User Story:** As a system, I want spawn rates to react to weather and time of day.
    *   **Implementation:** Implement `WorldStateManager` that holds `TimeOfDayEnum` and `WeatherEnum`, publishes context, and is consumed by `BirdSpawner` to weight `BirdDefinition.Variants`.
    *   **Acceptance:** Changing world context changes spawn probability without engine restart.

*   **[UI-002] Snap Decision Input Processing**
    *   **User Story:** As an expert player, I want to type or speak a direct bird guess.
    *   **Implementation:** Add a text input flow with string normalization and fuzzy matching (e.g., Levenshtein) against `BirdDatabase` IDs/names. Voice input can be stubbed with a mock provider.
    *   **Acceptance:** Near-miss spellings still resolve to the intended bird within a tolerance threshold.

*   **[UI-003] Life List / Journal UI**
    *   **User Story:** As a player, I want to browse my collected birds.
    *   **Implementation:** Scrollable grid of unlocked bird cards, pulling data from `LifeList` and showing the selected `PlumageVariant` thumbnail.
    *   **Acceptance:** Newly unlocked birds appear without restarting the session.

*   **[SYS-004] Encounter Outcome Scoring**
    *   **User Story:** As a player, I want graded rewards based on shot quality.
    *   **Implementation:** Wire `CalculatePhotoQuality` into the encounter resolution, mapping quality to Bronze/Silver/Gold rewards and XP.
    *   **Acceptance:** Automated tests validate reward tiers for center vs off-center reticle positions.

*   **[UI-001B] Encounter Result Screen**
    *   **User Story:** As a player, I want to see how well I did.
    *   **Implementation:** Create a UI overlay that displays the "Photo Quality" (Bronze/Silver/Gold) and XP gained.
    *   **Acceptance:** The correct medal is shown based on the score.

*   **[LOGIC-003] Taxonomic Progression & Variant Swap Logic**
    *   **User Story:** As a player, I want harder birds to demand deeper knowledge (dimorphism, molting, Empidonax) as I level up.
    *   **Implementation:** Add a pure `/Core` progression/variant selector that maps player level to allowed `PlumageVariant` pools (e.g., unlock Female/Molting variants, escalate to Empidonax silhouettes). Injects `WorldContext` and `DifficultyScaler` so asset swaps are data-driven, not engine-specific.
    *   **Acceptance:** Unit tests confirm the selector yields the correct variant pool per level and gracefully falls back when data is missing.

*   **[UI-005] Empidonax Challenge Mode (Silhouette/Audio-First)**
    *   **User Story:** As an expert, I want a mode where visual ID is obscured and I must rely on habitat and audio.
    *   **Implementation:** Add a UI state that hides/obscures the bird sprite (silhouette/blur), foregrounds spectrogram/audio hints, and routes inputs through the challenge flag from `DifficultyScaler`.
    *   **Acceptance:** Toggling the mode swaps the UI state; encounter resolution still works and awards appropriate XP/credits.

*   **[SYS-004B] Fog & Weather Risk Multipliers**
    *   **User Story:** As a player, I want riskier low-visibility conditions to pay better rewards.
    *   **Implementation:** Extend encounter scoring to apply configurable multipliers based on `WeatherEnum` (e.g., Fog, Rain) and `TimeOfDayEnum`. Expose tuning via `WorldContext` so both spawn weighting and scoring share the same source of truth.
    *   **Acceptance:** Tests verify multipliers apply only in low-visibility contexts and stack safely with `CalculatePhotoQuality` outputs.

*   **[UI-006] Spectrogram Accessibility Controls**
    *   **User Story:** As a hearing-impaired player, I want to toggle and enlarge spectrograms for identification.
    *   **Implementation:** Add UI affordances to toggle spectrogram overlays, open them full-screen, and provide keyboard/touch navigation. Backed by pre-baked spectrogram assets from `BirdDefinition.Calls`.
    *   **Acceptance:** Accessibility toggle works without restarting; spectrograms display correctly on desktop and mobile layouts.

### Sprint 3: AI & External APIs (The "Feather Quest" Magic)
*Goal: Connect to LLM and Real World Data.*

*   **[API-001] eBird Integration (Mock/Stub first)**
    *   **User Story:** As a player, I want to see real data.
    *   **Implementation:** Create `IEBirdService`.
        *   *Implementation A (Mock):* Returns fake list of "Rare Birds".
        *   *Implementation B (Live):* HTTPRequest node to eBird API 2.0. Includes pulling player's real-world checklist.

*   **[API-002] Ranger Gemma (LLM Chat)**
    *   **User Story:** As a player, I want to ask why I got the ID wrong.
    *   **Implementation:** HTTPRequest to OpenAI/Google API.
    *   **Prompt Engineering:** "You are Ranger Gemma, an expert ornithologist. Be encouraging, concise, and use Socratic questioning to help the player learn. The user just failed to identify a [CorrectBird]. They guessed [WrongBird]. Explain the difference based on field marks: [FieldMarksList]."

*   **[DATA-001] AI-to-Journal Pipeline**
    *   **User Story:** As a player, I want Gemma's advice saved to my journal.
    *   **Implementation:** Create `JournalService` that parses LLM responses and appends them to `PlayerProfile.JournalEntries`.
    *   **Acceptance:** Chat history persists in the Life List view for that bird.

*   **[SYS-010] Wildcard Token System**
    *   **User Story:** As a player, I want to use a token to bypass a failed ID.
    *   **Implementation:** Update `EncounterManager` to accept `ConsumeWildcard()`. Update UI to show "Use Wildcard" button when tokens are available.
    *   **Acceptance:** Using a token converts a failure to a success and decrements the token count.

---

### Sprint 4: Meta-Game, Economy & Systems
*Goal: Fill spec gaps: hub, economy, difficulty scaling, and seasonality.*

*   **[UI-004] Visitor Center Hub Shell**
    *   **User Story:** As a player, I need a central hub to launch practice mini-games and return without losing state.
    *   **Implementation:** Build the `VisitorCenter` shell scene (navigation, menu, launcher). Routes launches to dedicated mini-game presenters (see UI-004A–UI-004E) and reports results via `IMiniGameResultSink` into `ProgressionService`.
    *   **Acceptance:** Hub launches and exits any mini-game, preserves player state, and resumes music/ambience correctly.

*   **[UI-004A] Call Hero (Audio ID)**
    *   **User Story:** As a player, I want to practice bird call identification.
    *   **Implementation:** Time-boxed rounds that play `BirdCall.AudioPath`; player selects from multiple-choice answers. Uses `/Core` randomization seeded for repeatable tests.
    *   **Acceptance:** Correct answers award XP/credits; wrong answers show the spectrogram and playbacks; scores flow back to the hub.

*   **[UI-004B] Flashcard Frenzy (Field Marks)**
    *   **User Story:** As a player, I want quick-drill flashcards on field marks.
    *   **Implementation:** Presents sprite/snippet + field-mark prompt; player swipes/presses to answer. Uses `BirdDefinition.FieldMarks` and `PlumageVariant` assets.
    *   **Acceptance:** Session summary shows accuracy and streaks; completion posts results to `ProgressionService`.

*   **[UI-004C] Habitat Hunt**
    *   **User Story:** As a player, I want to match birds to habitats.
    *   **Implementation:** Lightweight scene with habitat cards; player drags/drops bird cues onto habitats. Consumes biome metadata from `/Core` (stubbed if absent).
    *   **Acceptance:** Correct matches award credits; incorrect matches provide hints referencing `WorldContext`.

*   **[UI-004D] Taxonomy Trees**
    *   **User Story:** As a player, I want to practice taxonomic ordering.
    *   **Implementation:** Mini-game asks players to arrange orders/families; uses taxonomy data from the bird database.
    *   **Acceptance:** Valid order awards XP; errors highlight correct hierarchy; supports keyboard and touch.

*   **[UI-004E] Field Mark Match**
    *   **User Story:** As a player, I want to match visual marks to the right bird under time pressure.
    *   **Implementation:** Timed matching grid that pairs field-mark icons/text to bird thumbnails; pulls assets from `PlumageVariant`.
    *   **Acceptance:** Combo/streak bonuses apply; results return to the hub.

*   **[SYS-005] Inventory & Shop Manager (Gear Upgrades)**
    *   **User Story:** As a player, I want to buy and equip stabilizers, lenses, and microphones that meaningfully change encounter mechanics.
    *   **Implementation:** In `/Core`, add models: `GearType { Stabilizer, Lens, Microphone }`, `Item`, `Inventory`, `ShopCatalog`, and services: `IShopService`, `IWallet`, `IInventoryService`. Add dual-gate biome unlock checks (Credits + Level) in a `BiomeUnlockService`. Expose gear effects to encounter math via a `TuningParams` object (e.g., `LensMultiplier`, `StabilityModifier`, `AudioGain`). Persist equipment in `PlayerProfile` through existing `SaveManager`.
    *   **Acceptance:** Purchasing decrements credits and adds the item to `Inventory`; equipping updates `PlayerProfile` and changes encounter math; biomes enforce the Credits+Level dual-gate.

*   **[SYS-006] The Sanctuary (3D Scene & Population Logic)**
    *   **User Story:** As a player, I want to walk a sanctuary populated by the birds I’ve identified.
    *   **Implementation:** In `/Core`, add `SanctuaryPlanner` that consumes `LifeList` and returns a deterministic placement plan (spawn positions, densities, time windows). In `GodotClient`, render a simple 3D/2.5D `Sanctuary` scene that instantiates passive `BirdDisplay` actors based on the plan (no combat/encounter logic). Include basic navigation and culling/pooling for performance.
    *   **Acceptance:** Newly unlocked birds appear in the sanctuary without a restart; player can navigate smoothly; population remains stable performance-wise on Web export.

*   **[LOGIC-002] Difficulty Scaler (Level-Based Tuning & Expert Challenges)**
    *   **User Story:** As a system, I want mechanics to scale as the player levels up and enable expert challenges (e.g., Empidonax relying on audio/habitat only).
    *   **Implementation:** Add a pure `/Core` service `DifficultyScaler` that maps `PlayerLevel` and `WorldContext` to `TuningParams` (e.g., `FocusFillRate`, `PatienceDrainRate`, `SwayAmplitude`, `SpawnWeights`). Provide curve tables or formulas for monotonic scaling. Add flags for expert challenge modes that selectively hide visual field marks and increase reliance on audio and habitat context.
    *   **Acceptance:** Unit tests verify monotonic parameter changes across levels; toggling expert mode hides visual marks and shifts filters/weights appropriately; encounter outcomes reflect tuned parameters.

*   **[SYS-007] Calendar & Season Manager (Seasons, Events, Alerts)**
    *   **User Story:** As a system, I need to track in-game date/season and publish events (e.g., Big Year, rare bird alerts) that influence spawn weights and UI.
    *   **Implementation:** In `/Core`, add `CalendarService` with in-game clock, seasons (Spring, Summer, Fall, Winter), and event scheduling. Publish updates on an `IEventBus`. Integrate with `WorldContext` to set seasonality and trigger migration waves (e.g., Warblers in May). Stub structures for community features (Big Year aggregation, Rare/Vagrant Alerts) that can later bind to a backend. Ensure deterministic simulation when seeded for tests.
    *   **Acceptance:** Changing date updates `WorldContext.Season` and affects spawn probabilities; scheduled events publish to listeners; rare bird alerts can be displayed by a client implementation without engine restart.

*   **[UI-007] Simulated Forum UI (Vagrant System)**
    *   **User Story:** As a player, I want to see alerts about rare birds.
    *   **Implementation:** Create `NotificationFeedUI` in the Visitor Center that persists the last 5 vagrant alerts from `CalendarService`.
    *   **Acceptance:** Alerts generated by the system appear in the UI feed.

*   **[LOGIC-004] Fallout Event Logic**
    *   **User Story:** As a player, I want to experience "The Fallout" event with massive bird numbers.
    *   **Implementation:** Add `EventState` to `BirdSpawner` allowing for temporary 10x-20x spawn rate multipliers when `WorldContext.Event == Fallout`.
    *   **Acceptance:** Triggering the event drastically increases spawn rates.

*   **[SYS-008] Basic Analytics Events (MVP)**
    *   **User Story:** As a product owner, I want baseline analytics to track engagement and funnels.
    *   **Implementation:** Add platform-agnostic `IAnalytics` interface with minimal events (session start, encounter start/end, purchase, mini-game results). Provide a stub/local logger implementation and a Segment/GA-capable implementation guarded for WebGL.
    *   **Acceptance:** Events fire without breaking offline mode; toggling analytics off is respected; payloads are validated in unit tests.

*   **[SYS-009] Storefront & Economy Modifiers (IAP-Ready)**
    *   **User Story:** As a business stakeholder, I want to sell credits/boosters safely across platforms.
    *   **Implementation:** Define `IStoreInterface` for platform IAP, mock implementation for Web, and `EconomyModifierService` to apply credit/XP boosters with durations. Ensure boosters stack rules and persistence via `SaveManager`.
    *   **Acceptance:** Boosters apply and expire correctly in tests; store calls are abstracted behind the interface and can be swapped per platform.

*   **[SCRIPT-001] First-Time User Experience (FTUE) / Tutorial**
    *   **User Story:** As a new player, I want a guided introduction to the game mechanics.
    *   **Implementation:** Create `TutorialDirector` that overrides `GameDirector` state for a scripted sequence (Fixed Spawn -> Forced Binoculars -> Guided ID).
    *   **Acceptance:** New save files trigger the tutorial sequence; returning players skip it.

---

### Phase 2: Post-Launch & Advanced Tech
*Goal: Capture advanced systems from the spec to be tackled after MVP stability.*

*   **[DEFERRED-001] Smart Director (RL/Bandit Difficulty)**
    *   **Intent:** Upgrade `DifficultyScaler` to a contextual bandit/RL agent that adapts spawn weights and tuning based on player performance.
    *   **Notes:** Requires telemetry from `IAnalytics`; must fall back to deterministic curves if the model is unavailable.

*   **[DEFERRED-002] Ranger Gemma + GraphRAG**
    *   **Intent:** Add a Neo4j/graph-backed knowledge layer to ground LLM responses, reducing hallucinations for identification feedback.
    *   **Notes:** Replace direct prompt approach from `API-002` with graph-augmented retrieval and caching.

*   **[DEFERRED-003] Migration Forecasting Engine**
    *   **Intent:** Predict rarity/arrival windows using historical weather + eBird data, feeding into `WorldContext` and spawn weighting.
    *   **Notes:** Must provide deterministic seeded simulation mode for tests.

*   **[DEFERRED-004] Data Lakehouse & Churn Prediction**
    *   **Intent:** Pipe analytics events to a warehouse/lakehouse for retention/churn modeling and cohort analysis.
    *   **Notes:** Builds on `SYS-008`; deferred to post-launch to avoid WebGL bloat.

*   **[DEFERRED-005] Edge Computer Vision Prototype**
    *   **Intent:** Explore TensorFlow.js/on-device CV for photo validation instead of math-based scoring.
    *   **Notes:** Only pursued if performance budgets allow; MVP remains math-only as noted in Risk Assessment.

*   **[DEFERRED-006] Voice-to-Text Integration**
    *   **Intent:** Allow players to speak their bird guesses instead of typing.
    *   **Notes:** Requires integration with Web Speech API or a cloud provider.

*   **[DEFERRED-007] Real-World Forum Integration**
    *   **Intent:** Pull from actual birding forums/listservs (e.g., eBird rare bird alerts) to mirror real-world vagrant sightings.
    *   **Notes:** Replaces the simulated forum UI.

*   **[DEFERRED-008] Global "Big Year" Events**
    *   **Intent:** Players worldwide contribute to community goals.
    *   **Notes:** Requires a robust backend for aggregating global stats.

---

## 5. Migration Strategy Checklist (For Month 6)

When the time comes to move to Unity, follow this protocol:
1.  **Copy** the `/Core` folder into the Unity Assets folder.
2.  **Rewrite** the `IInputProvider` implementation using Unity's `InputSystem`.
3.  **Rewrite** the `IAudioManager` using Unity's `AudioSource`.
4.  **Rebuild** the UI using Unity UI Toolkit or UGUI.
5.  **Re-link** the logic. The `FocusCalculator` and `BirdDatabase` code will remain 100% identical.

---

**Instruction to Coding Agent:**
*Start with [CORE-001]. Create the Solution with two projects: `FeatherQuest.Core` and `FeatherQuest.Godot`. Do not write any engine-specific code until the Data Models are established.*