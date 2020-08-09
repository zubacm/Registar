let connection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .configureLogging(signalR.LogLevel.Information)
    .build();

connection.start();

connection.on('send', (userid, data) => {
    console.log("odavde" + userid);
    var currentdate = new Date();
    var when =
        (currentdate.getMonth() + 1) + "/"
        + currentdate.getDate() + "/"
        + currentdate.getFullYear() + " "
        + currentdate.toLocaleString('en-US', { hour: 'numeric', minute: 'numeric', hour12: true });
    DisplayMessagesDiv = document.getElementById("DisplayMessages");
    if (userid === document.getElementById("senderid").value) {
        DisplayMessagesDiv.innerHTML += "<div class='outgoing_msg'><div class='sent_msg'><p>"
            + data + " </p> <span class='time_date'>" + when + "</span></div></div>";
    }
    else {
        DisplayMessagesDiv.innerHTML += "<div class='incoming_msg'><div class='received_msg'><div class='received_withd_msg'><p>"
            + data + " </p> <span class='time_date'>" + when + "</span></div></div></div>";
    }
    

    SendMessageInput = document.getElementById('txtMessage');
    SendMessageInput.value = "";
    $("#DisplayMessages").scrollTop($("#DisplayMessages")[0].scrollHeight);

});

function sendMessage() {
    var userid = document.getElementById("senderid").value;
    var msg = document.getElementById("txtMessage").value;
    connection.invoke('send', userid, msg);
}


