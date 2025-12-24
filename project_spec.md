# Project Title: Wing Watcher: Global Expedition (Working Title)
**Platform:** Web-Based Application (Responsive for Mobile Touch & Laptop Mouse/Keyboard)
**Genre:** Educational Simulation / Collection RPG
**Core Concept:** "Pokémon" meets "Real World Ornithology"
**Tone:** Cozy, Adventurous, Scientific, Rewarding

---

## 1. High-Level Design Pillars
1.  **Fun Over Rote Learning:** Education is the outcome, not the mechanic. The game must feel tactile ("Juicy") and exciting.
2.  **No Barriers to Play:** No energy systems, no time-gating. If the player wants to play for 6 hours, they can.
3.  **Low Floor, High Ceiling:** Accessible to someone who doesn't know a Robin from a Jay, but challenging enough for a veteran birder.
4.  **Real-World Integration:** Playable entirely virtually, but enhanced by real-world activity (eBird).

---

## 2. Advanced Progression Logic (Flow Theory)

### Difficulty Scaling
The game introduces complexity gradually to maintain engagement across skill levels:

*   **Beginner:** Bright male birds in breeding plumage (Cardinals, Blue Jays). High visual contrast.
*   **Intermediate:** Introducing **Sexual Dimorphism**. Female birds with drabber, harder-to-distinguish colors (e.g., female House Finch vs. female Purple Finch).
*   **Advanced:** **Molting Phases**. Birds in non-breeding plumage where key field marks may be absent or muted.
*   **Expert:** **The "Empidonax" Challenge**. Flycatcher species where visual ID is nearly impossible, forcing players to rely exclusively on:
    *   Habitat context (Willow vs. Alder habitats)
    *   Voice/Call patterns (Spectrogram analysis becomes mandatory)

### Mechanical Difficulty Scaling
Beyond taxonomic complexity, the physical mechanics of the encounter become more demanding:

*   **Speed Progression:** As the player earns more EXP and levels up, the "Focus Bar" fills at a **faster rate**, requiring quicker identification decisions.
*   **Time Pressure:** Higher-level encounters have shorter "Patience Meters"—the bird flushes faster, giving less time to observe.
*   **Movement Patterns:** Advanced birds move more erratically in the binocular view, making the reticle-tracking challenge harder.

### Why This Matters
This mirrors real-world birding progression. A veteran birder doesn't just know *more* birds—they know *harder-to-see aspects* of birds. The dual progression (taxonomic knowledge + mechanical skill) ensures the game remains challenging and engaging at all levels. This system teaches genuine field skills.

---

## 3. The Core Gameplay Loop

### Phase 1: Exploration (The Hunt)
*   **The View:** The player views a panoramic, scrolling biome (e.g., "Ohio Deciduous Forest").
*   **Cues:**
    *   *Visual:* Shaking leaves, flitting shadows, colors popping in the canopy.
    *   *Audio:* Authentic bird calls appear.
    *   *Spectrogram:* A visual waveform of the call appears on screen (Accessibility/Silent Play feature).
*   **Action:** Player clicks/taps the cue to raise Binoculars.

### Phase 2: The Encounter (The "Battle")
*   **The Mechanic:** A first-person binocular view.
*   **Stability System:** The camera sways slightly (simulating hands). Player must keep the reticle over the moving bird to fill the **"Focus Bar."**
*   **Patience Meter:** A hidden timer. If the player takes too long or moves erratically, the bird flushes (flies away).

### Phase 3: Identification (The Choice)
Once the Focus Bar is full, Time Dilation (Slow-Mo) activates. The player has two paths:

**Path A: The Detective (Guided)**
*   **Step 1:** Select Size (Sparrow, Robin, Crow, Goose).
*   **Step 2:** Select Colors (Interactive Color Wheel: Black, Gray, White, Brown/Buff, Red/Rufous, Yellow, Olive, Blue, Orange). Pick up to 3.
*   **Step 3:** Select Field Marks (Wing bars, Eye ring, Crest, etc.).
*   **Result:** Game filters database $\rightarrow$ Player selects bird from shortlist.
*   **Reward:** Standard XP + Conservation Credits.

**Path B: The Snap Decision (Expert/Risk)**
*   **Action:** Player hits "I Know This!" button.
*   **Input:** Text Box opens (with Voice-to-Text microphone enabled).
*   **Timer:** The Patience Meter stops immediately.
*   **Mechanic:** Player types/speaks the bird name (e.g., "Black-capped Chickadee").
*   **Result:** Instant Success or Failure.
*   **Reward:** **2.5x Multiplier** on XP and Credits. High Risk, High Reward.

### Phase 4: Resolution
*   **Success:** Bird added to Life List. Credits awarded.
    *   **Photography Rating:** The game assigns a "Quality Score" (Bronze/Silver/Gold) based on how well-centered the bird was in the binoculars during the Focus phase. Better ratings = bonus XP.
*   **Failure:** Bird flies away.
*   **The Safety Net (Ranger Gemma):** Player can initiate a chat with the LLM guide.
    *   *Available:* **Always**—not just on failure. Even if you succeed, Gemma can explain *why* your ID was correct or discuss interesting facts.
    *   *Prompt (Failure):* "I missed it! It had a red chest but a gray back?"
    *   *Gemma (Response):* "Sounds like an American Robin, but could have been a Towhee. Did you see its eyes?"
    *   *Prompt (Success):* "I got it! But was that just a lucky guess?"
    *   *Gemma (Response):* "Not at all! The white throat patch and chunky bill are textbook White-throated Sparrow. Well done!"
    *   **Field Note Generation:** After a conversation, Gemma can auto-generate a journal entry summarizing the sighting and learning moment, which gets saved to the player's "Field Journal."
    *   *Outcome:* Learning happens here. No Credits awarded for failed IDs, but the player gains knowledge for next time.

---

## 3. The "Visitor Center" (Game Hub)
*Physical location in the UI where players go to manage upgrades and grind currency.*

### Mini-Games (The Grinders)
*Reward players with **Conservation Credits** and **small amounts of EXP** (significantly less than field encounters, but still contributes to leveling).*

1.  **Call Hero:** Listen to audio/watch spectrograms. Match to bird.
2.  **Flashcard Frenzy:** Visual ID challenge in two formats:
    *   *Rapid-Fire Mode:* Birds flash on screen. Identify as quickly as possible.
    *   *Multiple Choice Mode:* One bird image, 4 species options. Select correct answer.
3.  **Habitat Hunt:** Drag and drop birds into their correct biomes.
4.  **Taxonomy Trees:** Link related species (Family/Genus).
5.  **Field Mark Match:** Highlight specific anatomical features (eye ring, wing bars, tail shape) on a silhouette and match to the correct species.

### The Store (Spending Credits)
*   **Travel Tickets (Biome Unlocks):** Purchase access to new maps/biomes (Costa Rica, New Zealand, etc.).
    *   **Dual-Gate System:**
        *   *Conservation Credits:* You must spend Credits to travel to a biome.
        *   *Player Level Requirement:* Each biome has a minimum level requirement. You cannot purchase access until you reach that level.
    *   **Philosophy:** This ensures players have sufficient experience and knowledge to identify regional species before entering advanced biomes. You can't "pay to skip" difficulty—you must earn proficiency.
*   **Field Guides:** Unlock the ability to ID birds in specific regions.
*   **Gear Shop:**
    *   *Stabilizers:* Reduce binocular sway.
    *   *Lenses:* Faster focus time.
    *   *Microphones:* clearer Spectrograms.
    *   *Scopes:* Zoom further with higher definition.

---

## 4. Systems & Architecture

### Environmental Systems (Dynamic World)
The biomes are not static—they respond to time and weather to create a living ecosystem:

*   **Time of Day:**
    *   *Dawn Chorus (5–7 AM):* Birds are most active. Increased spawn rates, louder calls.
    *   *Midday (11 AM–2 PM):* Birds go quiet and hide. Lower spawn rates, harder to spot.
    *   *Evening (5–7 PM):* Secondary activity spike (especially for thrushes, owls).
*   **Dynamic Weather:**
    *   *Rain:* Most birds hide. Water birds (ducks, herons) become more active.
    *   *Post-Rain:* Worms surface. Robins, thrushes, and woodcocks swarm lawns.
    *   *Fog:* Reduces visibility, but increases the challenge (and reward multiplier) for successful IDs.

### Seasonal Events & Community Goals

*   **"Big Year" Global Events:** Players worldwide contribute to community goals.
    *   *Example:* "Identify 5,000,000 Warblers during Spring Migration to unlock the Cerulean Warbler as a guaranteed spawn."
*   **Seasonal Migration Mechanics:**
    *   Real-world calendar integration: Spring brings waves of migrant songbirds (Warblers in May). Fall brings shorebirds and raptors.
    *   *The Fallout:* A rare event where hundreds of exhausted migrants land simultaneously after bad weather. High-intensity, high-reward gameplay.

### The "Rarity" & "Vagrant" System
*   **RNG Logic:** Every spawn has a 0.5% chance to be a "Vagrant" (a bird not native to that biome).
*   **The Alert:** The "Simulated Forum" notifies the player: *"RARE BIRD ALERT: Painted Bunting spotted in Sector 4 (Ohio)!"*
*   **Player Action:** Player rushes to that biome to try and find it before the event timer expires.

### The Sanctuary (Meta-Progression)
*   **The Visualizer:** A 3D, walk-able park.
*   **Population:** Every unique species the player has ID'd is present here.
*   **Interaction:** Clicking a bird in the sanctuary opens its detailed Pokedex entry (Distribution maps, photos, fun facts).

### AI & Data Integration
*   **eBird API:**
    *   *Pull:* Nightly sync. Checks player's real-world eBird checklist.
    *   *Reward:* If player spots a bird IRL, it:
        *   Unlocks a "Verified" stamp in-game (visual badge on that species).
        *   Awards **10x the Conservation Credits** of an in-game sighting.
        *   Grants a "Wildcard Token" (can be used to bypass one Failed ID).
    *   *Philosophy:* Real-world birding is the ultimate goal. The game should incentivize going outside.
*   **Real-World Forum Integration (Vagrant Alerts):**
    *   *Phase 1 (MVP):* Simulated forum posts for vagrant spawns.
    *   *Phase 2 (Post-Launch):* Pull from actual birding forums/listservs (e.g., eBird rare bird alerts, regional birding groups) to mirror real-world vagrant sightings in near real-time.
*   **LLM (Ranger Gemma):**
    *   Powered by Google Gemma (or OpenAI API).
    *   System Prompt: "You are an expert ornithologist. Be encouraging, concise, and use Socratic questioning to help the player learn."

---

## 5. UI/UX Rough Sketch

### The HUD (Heads Up Display - Field View)
*   **Top Left:** Location (e.g., "Presque Isle State Park, PA").
*   **Top Right:** Conservation Credits & Current Level.
*   **Bottom Center:** The "Binocular" Icon (Tap to engage).
*   **Bottom Right:** "Field Guide" (Pause menu/Reference).
*   **Overlay:** When a bird calls, a semi-transparent waveform (spectrogram) floats on the side of the screen.

### The "Analysis" Wheel (During ID)
*   Center of screen: The Bird (Frozen/Slow-mo).
*   Right side: The Input Panel.
    *   [Size Slider]
    *   [Color Wheel (9 segments)]
    *   [Field Mark Tags]
    *   [**"SNAP GUESS"** Button (Prominent, Glowing)]

---

## 6. Next Steps for Development (MVP - Minimum Viable Product)

1.  **Database Assembly:** Compile a JSON database of 50 common birds (Images, Audio, Taxonomy, Size, Colors, Field Marks, Difficulty Tier).
2.  **The Prototype:** Build the "Battle" encounter in a web engine (React/Three.js or Phaser).
    *   Test the "Focus" mechanic.
    *   Test the "Color Wheel" selection.
3.  **LLM Test:** Create a simple chat interface and test the prompt engineering for "Ranger Gemma" to ensure she gives helpful birding hints.

***

## 7. Advanced AI, Data, and Edge Systems

### 1. The "Smart Director" (Reinforcement Learning / Bandit Algorithms)
**The Concept:** Instead of hard-coding the "Difficulty Scaling" (Section 2), build a Multi-Armed Bandit or simple RL agent that manages the **Game Director**.
**The Feature:**
*   The system monitors the user’s "Flow State" (win/loss ratio, time to ID, session length).
*   If the user is succeeding too easily, the Director dynamically introduces tougher variables (fog, erratic movement, molting birds).
*   If the user is struggling/churning, the Director "pushes" a high-value, easier bird (a "pity spawn") to re-engage them.

### 2. The "Taxonomy Knowledge Graph" (Graph Databases & Advanced RAG)
**The Concept:** Enhancing Ranger Gemma (Section 4). LLMs sometimes hallucinate relationships.
**The Feature:**
*   Build a **Knowledge Graph (Neo4j or NetworkX)** mapping bird taxonomy (Order $\to$ Family $\to$ Genus $\to$ Species) and ecological relationships (e.g., *Blue Jay* --eats--> *Acorns*, *Blue Jay* --mobbing_behavior--> *Great Horned Owl*).
*   When the user asks Gemma a question, use **GraphRAG**. First, query the graph for ground-truth relationships, then feed that structured data into the Gemma system prompt.

### 3. "Eco-Forecasting" (Time Series & Geospatial Analysis)
**The Concept:** The "Migration Mechanics" (Section 4).
**The Feature:**
*   Don't just use a calendar. Create a predictive model using historical eBird data + historical weather data (OpenMeteo API).
*   Train a model (Prophet, LSTM, or XGBoost) to predict "Bird Casts" (migration intensity) based on wind direction and temperature drop.
*   **In-Game Implementation:** A "Forecast Map" layer where players can see a heatmap of predicted migration intensity for the next 3 in-game days.

### 4. Edge-Based Computer Vision (TensorFlow.js / ONNX)
**The Concept:** The "Photography Rating" (Section 3).
**The Feature:**
*   Instead of checking if the sprite is "centered" using simple coordinate math, implement a lightweight CNN (e.g., MobileNetV3) running in the browser (TensorFlow.js).
*   The model analyzes the "photo" the user took. It scores the image based on composition rules or "detectability" (i.e., is the key field mark visible?).
*   This moves the computation from the server to the client (Edge AI).

### 5. Data Lakehouse & User Analytics (Data Engineering / MLOps)
**The Concept:** Analyzing player behavior to improve the game.
**The Feature:**
*   Treat every click (ID attempt, binocular lift, menu navigation) as an event stream.
*   Pipe this data into a "Data Lakehouse" (e.g., DuckDB + S3 or BigQuery).
*   Build an automated **dbt (data build tool)** pipeline that aggregates this raw data into "Player Profile Features."
*   **Use Case:** Use this data to train a "Churn Prediction" model (offline) to see which bird species cause the most players to quit the game (too hard? unfair?).

***

### Designer's Note on "Monetization" (Optional)
Since you want to avoid energy gates and never block progress, monetization options include:

**Premium Subscription / One-Time Purchases:**
*   **Credit Boosters:** Earn Conservation Credits at a faster rate (e.g., +25% or +50% boost).
*   **Early Access Travel:** Unlock biomes at a slightly lower player level (e.g., unlock at Level 8 instead of Level 10).
*   **Cosmetics:** Custom skins for binoculars, decorations for the 3D Sanctuary, custom journal themes.

**Core Philosophy:**
*   **Never sell progress or ID solutions.** Free players can reach 100% completion—they'll just grind longer or play mini-games more.
*   **Monetization = Convenience, not Necessity.** Paying players progress faster, but non-paying players are never locked out of content or stuck behind paywalls.

Does this specification capture the full vision? If so, you are ready to start building your MVP!