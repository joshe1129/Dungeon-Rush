Wizards' Last Stand es un emocionante videojuego tipo Tower Defense, desarrollado en Unity, donde los jugadores deben defender su castillo de hordas de monstruos que emergen de un portal oscuro. Coloca estratégicamente torres con magos para detener a los invasores y salvar el reino de la destrucción.

Características del Juego
Jugabilidad: Coloca torres en el campo de batalla donde el jugador haga clic con el ratón.
Monstruos con IA: Los enemigos encuentran su camino automáticamente hacia el castillo usando un algoritmo de búsqueda en anchura (Breadth-First Search).
Algoritmo de Pathfinding
El sistema de pathfinding utiliza un algoritmo de búsqueda en anchura para permitir que los monstruos encuentren el camino más eficiente hacia el castillo, asegurando que el jugador no pueda bloquear completamente el camino al enemigo sin repercusiones.

Explicación Técnica
Inicio y Final: El algoritmo toma las coordenadas iniciales (portal) y finales (castillo) usando un sistema de nodos representados en una cuadrícula.
Exploración de Vecinos: Se exploran las coordenadas vecinas (arriba, abajo, izquierda, derecha) para encontrar los nodos caminables.
Búsqueda por Niveles: Se implementa un sistema de cola que prioriza la exploración en anchura, lo que garantiza que se encuentre el camino más corto.
Verificación de Bloqueo: Antes de que una torre sea colocada, el algoritmo verifica si la nueva colocación bloquearía el camino. Si es así, la torre no se puede ubicar en ese lugar.

Puedes probar Wizards' Last Stand directamente en este [enlace](https://joshe1129.itch.io/dungeon-rush)

Requisitos del Sistema
Unity 2021.3 o superior
C# para el scripting
Compatible con PC

Instalación
Clona este repositorio.
Abre el proyecto en Unity.
Presiona Play para comenzar a jugar.

Contribuciones
Las contribuciones son bienvenidas. Si quieres mejorar el algoritmo de pathfinding, añadir nuevos enemigos o torres, ¡no dudes en hacer un pull request!

