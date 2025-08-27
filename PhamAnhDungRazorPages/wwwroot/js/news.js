
var connection = new signalR.HubConnectionBuilder()
    .withUrl("/newshub")
    .build();


connection.on("NewsUpdated", function(action, data) {
    if (action === "created" || action === "updated") {
        console.log("News article " + action, data);
        location.reload(); // Reload page to reflect changes
    }
    else if (action === "deleted") {
        console.log("News article deleted: " + data);
        location.reload();
    }
});


connection.start().then(() => console.log("Connected to SignalR!"))
    .catch(err => console.error(err));
