

let connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start();

connection.on('send', (userid, receiverid, data) => {
    var currentdate = new Date();
    var when =
        (currentdate.getMonth() + 1) + "/"
        + currentdate.getDate() + "/"
        + currentdate.getFullYear() + " "
        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    DisplayMessagesDiv = document.getElementById("DisplayMessages");
    var currentuserid = getCurrentUserId();
    if (userid === currentuserid) {
        DisplayMessagesDiv.innerHTML += "<div class='outgoing_msg'><div class='sent_msg'><p>"
            + data + " </p> <span class='time_date'>" + when + "</span></div></div>";
    }
    else if (receiverid === currentuserid) { 

        DisplayMessagesDiv.innerHTML += "<div class='incoming_msg'><div class='received_msg'><div class='received_withd_msg'><p>"
            + data + " </p> <span class='time_date'>" + when + "</span></div></div></div>";
        setCookieForNotification();
        document.getElementById('message-logo').classList.add('text-danger');
        document.getElementById('message-number').style.display = "inline";
    }

    SendMessageInput = document.getElementById('txtMessage');
    SendMessageInput.value = "";
    $("#DisplayMessages").scrollTop($("#DisplayMessages")[0].scrollHeight);

});

function sendMessage() {
    var userid = document.getElementById("senderid").value;
    var receiverid = document.getElementById("receiverid").value;
    var msg = document.getElementById("txtMessage").value;
    connection.invoke('send', userid, receiverid, msg);
}


function setCookieForNotification() {
        $.ajax({
            type: 'GET',
            url: "/Home/SetNotificationTrue",
            dataType: 'html',
            contentType: "application/json; charset=utf-8",
            success: function () {
            },
            error: function () {
                alert('Greška.');
            }
        });
}
function getCurrentUserId() {
        var userid = $.ajax({
            type: 'GET',
            url: "/Auth/GetCurrentUserIdentId",
            dataType: 'html',
            async: false,
            global: false,
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                return response;
            },
            error: function () {
                alert('Greška prilikom pribavljanja podataka.');
            }
        }).responseText;

        
        return userid;
}

