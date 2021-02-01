var btn = document.getElementById("btn");
const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

var paramsString = document.location.search;
var searchParams = new URLSearchParams(paramsString);
var ItemId = searchParams.get("id");




let span = document.getElementById("desc");
let spanValue = document.getElementById("desc").innerHTML;

span.innerHTML = parseMarkdown(spanValue);

//mardown parser 
function parseMarkdown(markdownText) {
    const htmlText = markdownText
        .replace(/^### (.*$)/gim, '<h3>$1</h3>')
        .replace(/^## (.*$)/gim, '<h2>$1</h2>')
        .replace(/^# (.*$)/gim, '<h1>$1</h1>')
        .replace(/^\>(.+)/gm, '<blockquote>$1</blockquote>')
        .replace(/\*\*(.*)\*\*/gim, '<b>$1</b>')
        .replace(/\*(.*)\*/gim, '<i>$1</i>')
        .replace(/!\[(.*?)\]\((.*?)\)/gim, "<img alt='$1' src='$2' />")
        .replace(/\[(.*?)\]\((.*?)\)/gim, "<a href='$2'>$1</a>")
        .replace(/\n$/gim, '<br />')

    return htmlText.trim()
}










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