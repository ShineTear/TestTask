$(document).ready(function () {
    $("#blackout").dialog({
        autoOpen: false,
        modal: true
    });
    $(".select2").select2({
        allowClear: false,
        placeholder: "",
        theme: "bootstrap",
        language: "ru"
    })

    $("#AppartmentTable").DataTable();
});

$(document).on('change', '#selectStreet', function () {
    /*POST*/
    var dataToPost = "{ street:" + $("#selectStreet").val() + "}";

    $.ajax({
        type: "POST",
        url: '/Home/LoadHouseNumber?street=' + $("#selectStreet").val(),
        dataType: "json",
        contentType: 'json',
        data: {  },
        success: function (data) {
            var div = document.querySelector("#HouseCheck"),
        frag = document.createDocumentFragment(),
        select = document.createElement("select");
            select.name = "HouseNumber";
            select.className = "selectAddr";
            if (div.childElementCount > 0) {
                while (div.firstChild) {
                    div.removeChild(div.firstChild);
                }
            }

            select.options.add(new Option("", "", true, true));
            for (var i = 0; i < data.length; i++) {
                select.options.add(new Option(data[i], data[i]));
            }

            frag.appendChild(select);
            div.appendChild(frag);
        },
        error: function (xhr) {
            alert('error');
        }
    })
});

function showState(className, checkedName) {
    var elements = document.getElementsByClassName(className);
    for (var i = 0, length = elements.length; i < length; i++) {
        if (document.getElementById(checkedName).checked) {
            elements[i].style.display = 'table-row';
        } else {
            elements[i].style.display = 'none';
        }
    }
}

function createNewAppartment() {
    $.get('/Home/CreateAppartment/', function (data) {
        $('#result').html(data);
    });
}

function addNewMeter(id) {
    $.get('/Home/AddNewMeter/' + id, function (data) {
        $('#result').html(data);
    });
}

function addNewValue(id) {
    $.get('/Home/AddNewValue/' + id, function (data) {
        $('#result').html(data);
    });
}

function showAllChange(id) {
    $.get('/Home/ShowAllChange/' + id, function (data) {
        $('#result').html(data);
    });
}