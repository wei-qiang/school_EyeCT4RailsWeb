$(function () {
    $.get('/Remise/GetRemiseTrams', function(data) {
        for (var i = 0; i < data.length; i++) {
            var id = "#sector" + data[i].Sector.Spoor.toString() + data[i].Sector.Nummer.toString();
            $(id).text(data[i].TramNummer);
            $(id).addClass("blocked");
        }
    });

    $.get('/Remise/GetBlockedSectors', function(data) {
        for (var j = 0; j < data.length; j++) {
            var id = "#sector" + data[j].Spoor.toString() + data[j].Nummer.toString();
            $(id).addClass("blocked");
        }
    });
});