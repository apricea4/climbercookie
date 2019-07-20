

$("#firstbtn").click(function () {
    $.ajax({
        url: '/api/order/createorder/1/1'
    }).done(function (val) {
        alert(val);

    });
});


