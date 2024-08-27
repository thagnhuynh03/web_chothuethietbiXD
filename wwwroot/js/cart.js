$(document).ready(function () {
    $('.add_cart_btn').on('click', function (e) {
        e.preventDefault(); // Ngăn chặn hành vi mặc định của liên kết

        var url = $(this).data('url'); // Lấy URL từ thuộc tính data-url

        $.ajax({
            type: "POST",
            url: url,
            success: function (response) {
                if (response.success) {
                    if (response.message) {
                        alert(response.message);
                    } 
                    updateCartCount(response.cartCount); // Cập nhật số lượng giỏ hàng
                    updateCartPreview(); // Cập nhật phần preview giỏ hàng
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
