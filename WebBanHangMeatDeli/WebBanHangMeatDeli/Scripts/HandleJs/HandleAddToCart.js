
$(document).ready(function () {
    shoppingCount();
    $('body').on('click', '.btnAddCart', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        var quantity = 1;
        var getQuantity = $('#productQuantity').val();
        if (getQuantity > 1) {
            quantity = parseInt(getQuantity);
        }
        $.ajax({
            url: "/Cart/AddToCart",
            type: "POST",
            data: { id: id, quantity: quantity },
            success: function (rs) {
                if (rs.success) {
                    $('#shoppingCount').html(rs.count);
                }
                toastr.success(rs.message, "Thông báo")
            }
        });
    });
    $('body').on('click', '.btnDelete', function (e) {
        e.preventDefault();
        var id = $(this).data('id');
        deleteItem(id);
    })
    $('body').on('change', '.quantityItems', function (e) {
        e.preventDefault();
        var quantity = document.querySelectorAll(".quantityItems");
        var Array = [];
        quantity.forEach(e => {
            Array.push({
                id: e.dataset.id,
                quantity: e.value,
            });
        })
        Update(Array);
    })

})

function shoppingCount() {
    $.ajax({
        url: "/Cart/Count",
        type: "GET",
        success: function (rs) {
            $('#shoppingCount').html(rs.count)
        }
    })
}

function deleteItem(id) {
    $.ajax({
        url: "/Cart/Delete",
        type: "POST",
        data: { id: id },
        success: function (rs) {
            if (rs.success) {
                $('#row_' + id).remove();
            }
            shoppingCount();
            loadCart();
        }
    })
}
function loadCart() {
    $.ajax({
        url: "/Cart/LoadCart",
        type: "GET",
        success: function (rs) {
            $('#load-cart').html(rs)
        }
    })
}
function Update(items) {
    $.ajax({
        url: '/Cart/Update',
        type: 'POST',
        data: JSON.stringify(items),
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (data) {
            loadCart();
        }
    });
}