# 🌀 Lorenz Attractor Visualizer

An interactive **Lorenz attractor simulator** in Unity 🎮, designed to explore chaos, the butterfly effect, and velocity dynamics in a visually intuitive way.



https://github.com/user-attachments/assets/76d7efe5-4e04-4267-96d2-44bbb219e8cf



https://github.com/user-attachments/assets/95d4b545-8f63-478f-89a7-17e371ebb23c


---

## 🔬 Features

- **Numerical Integration:** Uses **RK4** (Runge-Kutta 4th order) for accurate time evolution of the Lorenz system.
- **Dynamic Mesh Rendering:** The attractor trail is drawn with a **Mesh** rather than a LineRenderer for performance (even with 25k+ points).
- **Per-Point Velocity Coloring:**  
  Visualize instantaneous **velocity magnitude** along the trail using a customizable **Gradient** 🎨.
- **Butterfly Effect Mode 🦋:**  
  Slight rounding of initial conditions demonstrates **sensitive dependence on initial conditions** — chaos in action!
- **Compare Two Attractors:**  
  Spawn multiple attractors (normal vs. butterfly-effect) to observe **divergence over time**.
- **Orbit Camera:**  
  Smooth, mouse-controlled orbit camera that automatically centers on the active attractor points.  

---

## ⚙️ Usage

1. **Setup Attractor:**
   - Create a **ScriptableObject** (`LorenzAttractorSettings`) for each attractor.
   - Configure:
     - `timeStep`
     - `initialPosition`
     - `maxPoints`
     - `velocityGradient`
     - `enableButterflyEffect` ✅ to apply rounding to initial conditions.
   
2. **Add Attractor to Scene:**
   - Attach `LorenzAttractor.cs` to a GameObject.
   - Assign the corresponding `LorenzAttractorSettings`.

3. **Camera Setup:**
   - Attach `OrbitCamera.cs` to your main camera.
   - Set `target` to one of the Lorenz attractor GameObjects.
   - Enable `Use Dynamic Center` to follow the attractor trail.

4. **Visual Exploration:**
   - Drag the mouse to rotate the camera.
   - Scroll to zoom in/out.
   - Watch attractors diverge in real time, colored by velocity magnitude.

---

## 📊 Scientific Notes

- The Lorenz system is defined by:

\[
\begin{cases}
\frac{dx}{dt} = \sigma (y - x) \\
\frac{dy}{dt} = x (\rho - z) - y \\
\frac{dz}{dt} = xy - \beta z
\end{cases}
\]

with typical parameters:  
`σ = 10`, `ρ = 28`, `β = 8/3`.

- **Butterfly Effect:** Small differences in initial conditions (here via rounding) grow exponentially, illustrating chaotic dynamics.
- **Velocity Visualization:**  
  Each vertex is colored according to the **magnitude of its instantaneous derivative**, showing where the system moves faster or slower in 3D space.

---

## 🧩 Scripts

- `LorenzAttractor.cs` — Generates and updates the attractor mesh in real-time.  
- `LorenzAttractorEquationsSolver.cs` — Implements RK4 integration and derivative calculation.  
- `LorenzAttractorSettings.cs` — ScriptableObject storing parameters, including butterfly effect option.  
- `OrbitCamera.cs` — Smooth orbit camera centered on the attractor’s active points.

---

## 🚀 Demo Ideas

- Spawn two attractors:
  - One normal
  - One with butterfly effect enabled
- Observe divergence over time 🦋.
- Experiment with gradient coloring to highlight **high-velocity regions**.
- Increase `maxPoints` for high-resolution trails.

---

## 💻 Requirements

- Unity 2023+ (or compatible version)  
- Standard 3D pipeline  
- Optional: HDRP/URP for improved visuals  
