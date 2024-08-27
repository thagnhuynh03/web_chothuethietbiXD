function toggleDropdown() {
    var dropdown = document.getElementById("dropdown-filter");
    if (dropdown.style.display === "none" || dropdown.style.display === "") {
        dropdown.style.display = "flex";
    } else {
        dropdown.style.display = "none";
    }
}
function toggleDropdownMenu() {
    var dropdownMenu = document.getElementById("dropdown-menu");
    if (dropdownMenu.style.display === "none" || dropdownMenu.style.display === "") {
        dropdownMenu.style.display = "flex";
    } else {
        dropdownMenu.style.display = "none";
    }
}
document.addEventListener('DOMContentLoaded', function () {
    const rows = document.querySelectorAll('tbody tr');
    rows.forEach(row => {
        const statusCell = row.querySelector('td span');
        if (statusCell.textContent.trim() === 'Đã xác nhận') {
            statusCell.style.backgroundColor = 'green';
            statusCell.style.color = 'white';
        } else if (statusCell.textContent.trim() === 'Đang chờ xác nhận') {
            statusCell.style.backgroundColor = '#a74444d7';
            statusCell.style.color = 'white';
        }
    });
});

document.getElementById('txtSearch').onkeypress = function (e) {
    if (e.keyCode === 13) {
        document.getElementById('btnSearch').click();
        return false; // Prevent form submission
    }
};