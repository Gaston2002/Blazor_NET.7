function pruebaPuntoNetStatic() {
    DotNet.invokeMethodAsync("BlazorPelicula.Client", "ObtenerCurrentCount")
        .then(resultado => {
            console.log('conteo desde javascript ' + resultado);
        })
}

function pruebaPuntoNetInstancia(dotnetHelper) {
    dotnetHelper.invokeMethodAsync("IncrementCount");
}