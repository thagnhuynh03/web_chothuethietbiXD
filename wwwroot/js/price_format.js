document.addEventListener("DOMContentLoaded", function () {
    // Lấy tất cả các phần tử có class 'price'
    var prices = document.querySelectorAll('.price');

    prices.forEach(function (priceElement) {
        // Lấy nội dung số tiền từ phần tử và chuyển đổi nó thành kiểu số
        var price = parseFloat(priceElement.textContent);

        // Định dạng số tiền thành tiền Việt Nam với dấu phân cách hàng nghìn và thêm ký hiệu ₫
        var formattedPrice = price.toLocaleString('vi-VN') + '₫';

        // Cập nhật nội dung của phần tử với số tiền đã được định dạng
        priceElement.textContent = formattedPrice;
    });
});