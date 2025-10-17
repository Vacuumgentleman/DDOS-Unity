# DedicatedServer-Unity

**Autor:** Vacuumgentleman  
**Lenguaje:** C#  
**Motor:** Unity  
**Servidor:** Docker (local)

---

## 🧠 Descripción general
**DedicatedServer-Unity** es un proyecto de Unity diseñado para probar y simular entornos multijugador con un servidor dedicado.  
Permite enviar y recibir datos de posición entre varios jugadores conectados a un servidor local.  
También puede ser utilizado para pruebas de rendimiento o simulaciones de carga, como ataques DDoS controlados, en entornos seguros.

---

## ⚙️ Características principales
- Sincronización de posiciones entre jugadores a través de peticiones HTTP (GET y POST).
- Envío periódico de datos mediante corrutinas.
- Soporte para múltiples jugadores y escenas independientes.
- Integración con un servidor externo en **Docker**.
- Códigos totalmente en **C#**, escritos desde cero.

---

## 🧩 Estructura de scripts principales

### `ApiClient.cs`
Gestiona las peticiones al servidor (`GET` y `POST`), incluyendo el envío automático de datos de posición y el evento `OnDataReceived`.

### `GameManager.cs`
Administra la lógica principal del juego, los jugadores conectados y la comunicación con el `ApiClient`.

### `PlayerController.cs`
Control básico de posición y movimiento de cada jugador en escena.

### `PositionSync.cs`
Permite sincronizar la posición de un jugador local con otro jugador remoto usando el servidor.

### `ServerData.cs`
Clase serializable para representar la información de posición enviada y recibida desde el servidor.

---

## 🧱 Escenas
- `PlayerA.unity` → Jugador local.
- `PlayerB.unity` → Jugador remoto.

Cada escena utiliza los mismos scripts pero con IDs de jugador diferentes para simular múltiples conexiones.

---

## 🐳 Servidor (Docker)
El servidor fue creado y ejecutado por aparte usando **Docker**, escuchando en el puerto `5005`.

Ejemplo de endpoint usado:
http://localhost:5005/server/


---

## 🚀 Objetivo del proyecto
El propósito de **DedicatedServer-Unity** es servir como entorno de prueba para:
- Evaluar latencia entre cliente y servidor.
- Medir consumo de red y CPU bajo múltiples peticiones simultáneas.
- Experimentar con comportamientos tipo DDoS en un entorno controlado.

---

## 💡 Próximas mejoras
- Métricas automáticas de rendimiento (RAM, CPU, ping).
- UI de monitoreo en tiempo real dentro de Unity.
- Soporte WebSocket para comunicación en tiempo real.

---

## 👨‍💻 Autor
**Vacuumgentleman**  
Desarrollador y experimentador de entornos multijugador en Unity.

---
