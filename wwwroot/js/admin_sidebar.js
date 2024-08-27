document.querySelectorAll('.sidebar_link_group_container').forEach(item => {
    item.querySelector('.sidebar_link_group_label')
        .addEventListener('click', () => {
            item.classList.toggle('active')
        })
})

document.querySelectorAll('.sidebar_link_group').forEach(item => {
    item.querySelectorAll('.sidebar_link').forEach(link => {
        if (link.classList.contains('active')) {
            item.parentElement.classList.add('active')
        }
    })
})
