// Módulo compartido de comunicación con la API REST
const API_BASE = "/api";

const ICONO_BEBIDA = "data:image/svg+xml;utf8," + encodeURIComponent(
    '<svg xmlns="http://www.w3.org/2000/svg" width="44" height="44" viewBox="0 0 24 24" fill="none" stroke="#60a5fa" stroke-width="2"><path d="M6 3h12l-1.5 18h-9L6 3z"/><path d="M6 8h12"/></svg>'
);

const ICONO_PLATO = "data:image/svg+xml;utf8," + encodeURIComponent(
    '<svg xmlns="http://www.w3.org/2000/svg" width="44" height="44" viewBox="0 0 24 24" fill="none" stroke="#34d399" stroke-width="2"><circle cx="12" cy="12" r="9"/><circle cx="12" cy="12" r="5"/></svg>'
);

function formatearMoneda(valor) {
    return new Intl.NumberFormat("es-CO", {
        style: "currency",
        currency: "COP",
        maximumFractionDigits: 0
    }).format(valor ?? 0);
}

async function consumirApi(endpoint, opciones = {}) {
    const config = {
        headers: { "Content-Type": "application/json" },
        ...opciones
    };

    const respuesta = await fetch(`${API_BASE}${endpoint}`, config);

    if (respuesta.status === 204) {
        return null;
    }

    const texto = await respuesta.text();
    let datos = null;
    try {
        datos = texto ? JSON.parse(texto) : null;
    } catch {
        datos = { mensaje: texto };
    }

    if (!respuesta.ok) {
        const mensajeError =
            datos?.mensaje ||
            datos?.Mensaje ||
            datos?.title ||
            `Error HTTP ${respuesta.status} al comunicarse con el servidor.`;
        throw new Error(mensajeError);
    }

    return datos;
}

function mostrarAlerta(contenedor, mensaje, tipo = "error") {
    if (!contenedor) return;
    contenedor.textContent = mensaje;
    contenedor.className = `status-box ${tipo === "error" ? "status-error" : "status-success"}`;
    contenedor.classList.remove("hidden");
}

function ocultarAlerta(contenedor) {
    if (!contenedor) return;
    contenedor.classList.add("hidden");
    contenedor.textContent = "";
}
