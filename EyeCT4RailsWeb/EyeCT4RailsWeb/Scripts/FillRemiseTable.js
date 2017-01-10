$(function () {
    $.get('/Remise/GetRemiseTrams', function(data) {
        console.log(data);
        for (var i = 0; i < data.length; i++) {
            var id = "#sector" + data[i].Sector.Spoor.toString() + data[i].Sector.Nummer.toString();
            $(id).text(data[i].TramNummer);
        }
        alert("trams loaded");
    });

    $.get('/Remise/GetBlockedSectors', function(data) {
        console.log(data);
        for (var j = 0; j < data.length; j++) {
            var id = "#sector" + data[j].Spoor.toString() + data[j].Nummer.toString();
            $(id).addClass("blocked");
        }
        alert("sectors blocked");
    });
});