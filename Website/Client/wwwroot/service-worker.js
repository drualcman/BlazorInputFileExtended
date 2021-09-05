self.addEventListener("install", async event => {
    console.log("Instalando el service worker...");
    self.skipWaiting();
});

self.addEventListener("fetch", event => {
    //podemos agregar logica personalizada para controlar
    //si se pueden utilizar los datos en cache cuando la aplicacion
    //se ejecute fuera de linea

    return null;
});