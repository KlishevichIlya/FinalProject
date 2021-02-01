var container = document.getElementById('container');
var input = document.querySelector('input[name=tagify]');
var tagify = new Tagify(input)
var set = new Set();
input.onchange = function () {
    var tags = document.getElementsByTagName('tag');
   
    for (i = 0; i < tags.length; i++) {
        //console.log(tags[i].innerText);
        set.add(tags[i].innerText);
    }
     
};
let button = document.getElementById('sendButton');
button.onclick = function () {
    for (let value of set) {
        var ip = document.createElement("input");
        ip.type = "text";
        ip.name = "tags";
        ip.setAttribute("value", value);
        ip.hidden = true;
        container.appendChild(ip);
    }

    
}


