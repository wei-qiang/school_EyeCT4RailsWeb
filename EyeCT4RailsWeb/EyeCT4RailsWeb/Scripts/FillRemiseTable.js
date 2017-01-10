$(function () {
    $.get('/Remise/GetRemiseTrams', function(data) {
        console.log(data);
        for (var i = 0; i < data.length; i++) {
            var id = "#sector" + data[i].Sector.Spoor.toString() + data[i].Sector.Nummer.toString();
            $(id).text(data[i].TramNummer);
        }
        alert("trams loaded");
    });
});