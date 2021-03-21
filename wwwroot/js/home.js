function showPart1() {
    $.ajax({
        type: "POST",
        url: "/Home/viewPart1",
        success: function (data) {
            $('#tableArea').empty();
            $('#tableArea').append(data);
        },
        error: function (error) {
            console.log(JSON.stringify(error))
        }
    })

};

function showPart2() {
    $.ajax({
        type: "POST",
        url: "/YonetimPaneli/viewTalepPart",
        success: function (data) {
            $('#tableArea').empty();
            $('#tableArea').append(data);
        },
        error: function (error) {
            console.log(JSON.stringify(error))
        }
    })

};

function showPart3() {
    $.ajax({
        type: "POST",
        url: "/YonetimPaneli/viewDurumPart",
        success: function (data) {
            $('#tableArea').empty();
            $('#tableArea').append(data);
        },
        error: function (error) {
            console.log(JSON.stringify(error))
        }
    })

};

function showPart4() {
    $.ajax({
        type: "POST",
        url: "/YonetimPaneli/viewNotPart",
        success: function (data) {
            $('#tableArea').empty();
            $('#tableArea').append(data);
        },
        error: function (error) {
            console.log(JSON.stringify(error))
        }
    })

};

function showPart5() {
    $.ajax({
        type: "POST",
        url: "/YonetimPaneli/viewSmsPart",
        success: function (data) {
            $('#tableArea').empty();
            $('#tableArea').append(data);
        },
        error: function (error) {
            console.log(JSON.stringify(error))
        }
    })

};

var header = document.getElementById("btnParents");
var btns = header.getElementsByClassName("btn");
for (var i = 0; i < btns.length; i++) {
    btns[i].addEventListener("click", function () {
        var current = document.getElementsByClassName("active");
        current[0].className = current[0].className.replace(" active", "");
        this.className += " active";
    });
}

$(document).ready(function () {

    $("#firstPart").click();

});