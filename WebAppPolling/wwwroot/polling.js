let intervalId;
var theKey = "";
var intento = 0;
poll = (key) => {
    fetch(`/Operacion/polling/${theKey}`)
        .then(response => {
                
                if (response.status === 200) {
                    response.json().then(j => {
                        const statusDiv = document.getElementById("status");
                        statusDiv.innerHTML = j.item;
                            clearInterval(intervalId);
                    });
            }
            if (response.status === 204) {
                intento = intento + 1;
                const statusDiv = document.getElementById("status");
                statusDiv.innerHTML = "No encontrado: " + intento;
            }
            if (response.status === 500) {
                clearInterval(intervalId);
                const statusDiv = document.getElementById("status");
                statusDiv.innerHTML = "Error";
                }
            }
        );
}

document.getElementById("submit").addEventListener("click", e => {
    e.preventDefault();
    const message = document.getElementById("product").value;
    fetch("/Operacion/polling/" + message,
        {
            method: "POST",
            //body: { product, size }
        })
        .then(response => {
            if (response.status === 200) {
                response.json().then(j => {
                    const statusDiv = document.getElementById("status");
                    statusDiv.innerHTML = j.item;
                    theKey = statusDiv.innerHTML;
                });
            }
        })
        .then(id => intervalId = setInterval(poll, 25000, id));
});