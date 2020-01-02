var myinput = document.getElementsByClassName('myinput');
for (var i = 0; i < myinput.length; i++) {
    myinput[i].oninvalid = function (e) { e.target.setCustomValidity('Ovo polje je obavezno.'); };
    myinput[i].oninput = function (e) { e.target.setCustomValidity(''); };
}

var myemailinput = document.getElementsByClassName('mymailvalidation');
for (var j = 0; j < myemailinput.length; j++) {
    myemailinput[j].oninvalid = function (e) { e.target.setCustomValidity('Molimo vas da unesete ispravan format za email adresu.'); };
    myemailinput[j].oninput = function (e) { e.target.setCustomValidity(''); };
}

function validatePassword() {
    var newpassword = document.getElementById("new-password");
    var confirmpassword = document.getElementById("confirm-password");
    if (confirmpassword.localeCompare(newpassword))
    {
        alert('wrong');
        document.getElementById("con-pass-validation").innerHTML = "Lozinka se ne poklapa.";
        return false;
    }
    alert('hej');
    return true;
}