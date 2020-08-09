var connection = new signalR.HubConnectionBuilder()
    .withUrl('/Home/Conversation')//, {
        //skipNegotiation: true,
        //transport: signalR.HttpTransportType.WebSockets})
    .build();

connection.on('receiveMessage', addMessageToChat);

connection.start()
    .catch(error => {
        console.error('ejjj'+error.message);
    });

function sendMessageToHub(message) {
    connection.invoke('sendMessage', message);
}