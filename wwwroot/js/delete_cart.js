$(document).ready(function () {
    $('.form_delete_cart').on('submit', function (e) {
        e.preventDefault(); // Ngăn chặn hành vi mặc định của form

        var $form = $(this);
        var url = $form.attr('action');
        var data = $form.serialize(); // Lấy dữ liệu từ form

        $.ajax({
            type: "POST",
            url: url,
            data: data,
            success: function (response) {
                if (response.success) {
                    // Xóa phần tử DOM tương ứng với sản phẩm đã xóa

                    // Cập nhật giỏ hàng (số lượng và preview) nếu cần thiết
                    updateCartCount(response.cartCount);
                    updateCartPreview();
                } else {
                    alert(response.message); // Hiển thị thông báo lỗi từ server
                }
            },
            error: function () {
                alert("Đã xảy ra lỗi. Vui lòng thử lại.");
            }
        });
    });

    function updateCartCount(count) {
        // Tìm phần tử <sup> bên trong .header_center_cart_icon
        const cartCountElement = document.querySelector('.header_center_cart_icon sup');
        if (cartCountElement) {
            cartCountElement.textContent = count;
        } else {
            console.error('Cart count element not found!');
        }
    }

    function updateCartPreview() {
        var cartPreviewUrl = $('#Cart_update').data('url');
        $.ajax({
            url: cartPreviewUrl,
            success: function (data) {

                $('.cart_previewer_product').remove();
                $('.cart_previewer_products').html(data); // Cập nhật nội dung giỏ hàng

            },
            error: function (xhr, status, error) {
                console.error('Error updating cart preview: ' + error);
            }

        });
    }
});