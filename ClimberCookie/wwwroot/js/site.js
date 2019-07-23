

$("#firstbtn").click(function () {
    $.ajax({
        url: '/api/order/createorder/1/1'
    }).done(function (val) {
        alert(val);

    });
});


$('#btn-login').click(function () {

    var val = document.getElementsByName("UserName")[0].value;
    var val2 = document.getElementsByName("Password")[0].value;
    $.ajax({
        url: '/api/security/login/val/val2'
    }).done(function (val) {
        alert(val);
    });

});