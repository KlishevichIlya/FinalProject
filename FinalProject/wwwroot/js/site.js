var chechbox = document.getElementById('selectTheme');
document.cookie = "theme=" + chechbox.checked;

var a = get_cookie("theme");
console.log(a);
//if (a == true) {
//    chechbox.checked = true;
//}



chechbox.addEventListener('change', function (){
    if (chechbox.checked == true) {        
        console.log(chechbox.checked);
        CookiesDelete();
        document.cookie = "theme=" + chechbox.checked;
        
    }
    else {        
        console.log(chechbox.checked);
        CookiesDelete();
        document.cookie = "theme=" + chechbox.checked;
       
    }
});

function CookiesDelete() {
    var cookies = document.cookie.split(";");
    for (var i = 0; i < cookies.length; i++) {
        var cookie = cookies[i];
        var eqPos = cookie.indexOf("=");
        var name = eqPos > -1 ? cookie.substr(0, eqPos) : cookie;
        document.cookie = name + "=;expires=Thu, 01 Jan 1970 00:00:00 GMT;";
        document.cookie = name + '=; path=/; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }
}




function get_cookie(cookie_name) {
    var results = document.cookie.match('(^|;) ?' + cookie_name + '=([^;]*)(;|$)');

    if (results)
        return (unescape(results[2]));
    else
        return null;
}