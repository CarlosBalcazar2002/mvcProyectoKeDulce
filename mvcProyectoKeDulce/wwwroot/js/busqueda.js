// Mostrar/ocultar cuadro de texto y botones
document.getElementById("btnMostrarBusqueda").addEventListener("click", function () {
    document.getElementById("inputBusqueda").style.display = "block";
    document.getElementById("btnMostrarBusqueda").style.display = "none";
    document.getElementById("btnBuscar").style.display = "inline-block";
});

// Ocultar cuadro de texto y botones si se hace clic fuera de ellos
document.addEventListener("click", function (event) {
    var inputBusqueda = document.getElementById("inputBusqueda");
    var btnMostrarBusqueda = document.getElementById("btnMostrarBusqueda");
    var btnBuscar = document.getElementById("btnBuscar");

    if (event.target.id !== "inputTextoBusqueda" && event.target.id !== "btnMostrarBusqueda" && event.target.id !== "btnBuscar") {
        inputBusqueda.style.display = "none";
        btnMostrarBusqueda.style.display = "inline-block";
        btnBuscar.style.display = "none";
    }
});

// Evitar que el clic en el cuadro de texto oculte el cuadro de texto
document.getElementById("inputTextoBusqueda").addEventListener("click", function (event) {
    event.stopPropagation();
});