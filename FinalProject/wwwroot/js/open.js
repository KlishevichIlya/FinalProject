var btn = document.getElementById("btn");
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

var paramsString = document.location.search;
var searchParams = new URLSearchParams(paramsString);
var ItemId = searchParams.get("id");


function Toogle() {
    if (btn.classList.contains("far")) {
        btn.classList.remove("far");
        btn.classList.add("fas");
    }
    else {
        btn.classList.remove("fas");
        btn.classList.add("far");
    }
}


hubConnection.on("Notify", function (comments) {
    console.log(comments);
    comments.forEach(com => {
        if (com.itemId == ItemId) {
            let elem = document.createElement("p");
            elem.appendChild(document.createTextNode(com.nick));
            elem.appendChild(document.createTextNode(" : "));
            elem.appendChild(document.createTextNode(com.text));
            let firstElem = document.getElementById("chatroom").firstChild;
            document.getElementById("chatroom").insertBefore(elem, firstElem);
        }


    })
    //console.log(sum);
   
});
   

hubConnection.on("Send", function (data, userName) {

    let elem = document.createElement("p");   
    elem.appendChild(document.createTextNode(userName));   
    elem.appendChild(document.createTextNode(" : "));
    elem.appendChild(document.createTextNode (data));
    let firstElem = document.getElementById("chatroom").firstChild;
    document.getElementById("chatroom").insertBefore(elem, firstElem);

});

document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;
    let nick = document.getElementById("nick").value;
    hubConnection.invoke("Send",nick, message,ItemId);
});

hubConnection.start();