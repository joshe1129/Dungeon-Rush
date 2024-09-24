# Dungeon Rush 🏰🧙‍♂️

¡Bienvenido a **Dungeon Rush**! Un emocionante juego de Tower Defense donde defenderás tu castillo de interminables hordas de monstruos que emergen de un portal oscuro. Coloca estratégicamente torres mágicas y lidera a los magos para detener la invasión y salvar el reino de la destrucción.

---

## 🌟 Características del Juego
- **Defensa Estratégica:** Coloca torres en el campo de batalla haciendo clic con el ratón y prepara tus defensas.
- **Enemigos con IA Avanzada:** Los monstruos siguen un camino hacia el castillo usando un sofisticado algoritmo de búsqueda en anchura (*Breadth-First Search*).
- **Sistema de Pathfinding:** Asegúrate de posicionar tus torres sin bloquear completamente el camino enemigo, ya que el sistema de pathfinding garantiza que siempre haya un paso libre.
  
---

## ⚙️ Mecánicas de Juego
- **Colocación de Torres:** Haz clic para posicionar torres estratégicamente en el campo de batalla y frenar la invasión.
- **Monstruos Inteligentes:** Los enemigos encuentran su camino automáticamente hacia el castillo, creando un desafío constante para tus defensas.
- **Verificación de Bloqueo:** Antes de colocar una torre, se verificará si obstruye el camino. Si lo bloquea por completo, no podrá ser colocada.
  
---

## 🔧 Explicación Técnica del Pathfinding
- **Inicio y Final:** El algoritmo de búsqueda en anchura toma las coordenadas del portal (inicio) y del castillo (final).
- **Exploración de Vecinos:** Analiza las posiciones adyacentes (arriba, abajo, izquierda, derecha) para determinar los caminos válidos.
- **Búsqueda por Niveles:** Utiliza una cola para garantizar la exploración en anchura, asegurando que siempre se encuentre el camino más corto.
- **Prevención de Bloqueos:** Verifica si las nuevas torres bloquean completamente el paso antes de colocarlas.

---

## 🚀 Instalación
1. Clona este repositorio.
2. Abre el proyecto en Unity (versión 2021.3 o superior).
3. Haz clic en **Play** para comenzar a jugar.

O pruébalo directamente en el siguiente enlace:  
👉 [Juega Dungeon Rush en Itch.io](https://joshe1129.itch.io/dungeon-rush)

---

## 💻 Requisitos del Sistema
- **Motor:** Unity 2021.3 o superior.
- **Lenguaje:** C# para el scripting.
- **Plataforma:** Compatible con PC.

---

## 🤝 Contribuciones
¡Las contribuciones son bienvenidas! Si quieres mejorar el algoritmo de pathfinding, añadir nuevos enemigos o torres, no dudes en hacer un *pull request*.

---

¡Prepárate para defender el reino en **Dungeon Rush**!
