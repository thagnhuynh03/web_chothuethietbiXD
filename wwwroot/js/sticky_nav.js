document.addEventListener("DOMContentLoaded", function () {
    var header = document.querySelector("header");
    var stickyNav = document.getElementById("sticky_nav");
    var stickyMenu = document.getElementById("sticky_menu");
    var menuButton = document.querySelector(".menu_button");

    var headerHeight = header.offsetHeight; // Chiều cao của header

    // Xử lý sự kiện cuộn trang
    window.addEventListener("scroll", function () {

        if (document.body.scrollTop > headerHeight || document.documentElement.scrollTop > headerHeight) {
            stickyNav.style.top = "0";
        } else {
            stickyNav.style.top = "-200px";
        }
    });
    // Xử lý sự kiện click nút menu
    menuButton.addEventListener("click", function () {
        if (stickyMenu.classList.contains("hidden")) {
            stickyMenu.classList.remove("hidden");
        } else {
            stickyMenu.classList.add("hidden");
        }
    });

    document.addEventListener("click", function (event) {
        var isClickInside = stickyNav.contains(event.target) || menuButton.contains(event.target);
        if (!isClickInside) {
            stickyMenu.classList.add("hidden");
        }
    });
});
