# 🧱 Crate & Cards

The focus of this implementation is:
- Smooth movement
- Clean architecture
- Responsive gameplay feedback
- Optimized object lifecycle management

---

## 🎮 Gameplay Overview

The player interacts with crates containing colored cards.

### Core Loop

1. Player taps on a crate.
2. Cards move out onto a conveyor path.
3. Cards move smoothly along a conveyor.
4. When a card reaches a matching-color crate:
   - It jumps into the crate.
5. When a crate becomes full:
   - It plays a feedback animation.
   - It disappears from the scene.

The goal was to match the feel, smoothness, and clarity of the reference mechanic.

---

## 🧩 Features

- ✅ Tap-based input system  
- ✅ Accurate color-matching logic  
- ✅ Conveyor movement using Dreamteck Splines  
- ✅ Smooth animations using DOTween  
- ✅ Crate capacity & full-state system  
- ✅ Crate spawn/despawn state validation  
- ✅ Object Pooling (Cards, Crates, Audio)  
- ✅ Centralized Sound Manager  
- ✅ Modular & scalable architecture  

---

## 🏗️ Architecture Overview

The project is structured with a separation of responsibilities and scalable systems.

### Core Systems

### 🔹 Card System
- Controls movement on the spline
- Handles jump-to-crate animation
- Maintains color identity
- Communicates with crate logic

### 🔹 Crate System
- Stores cards
- Maintains capacity
- Prevents card attachment when:
  - Full
  - Spawning
  - Despawning
- Handles full animation & despawn

### 🔹 Conveyor System
- Built using Dreamteck Splines
- Smooth spline-based movement
- Configurable speed

### 🔹 Object Pool System
- Reusable pooled objects
- Avoids runtime instantiation spikes
- Improves performance and memory management

### 🔹 Centralized Sound Manager
- Single entry point for SFX
- Prevents overlapping sound duplication
- Uses pooled audio sources

---

## 🛠️ Tools & Packages Used

- DOTween (Animations)
- Dreamteck Splines (Conveyor Path & Spline Mesh)
- URP Simple Toon Shader
- Unity 6.3 LTS (URP)

---
