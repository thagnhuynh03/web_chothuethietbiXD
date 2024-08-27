document.querySelector('.search_icon_button').addEventListener('click', e => {
    e.stopPropagation();
    const searchbar = document.querySelector('.header_center_bottom')
    searchbar.classList.toggle('show')
    if (searchbar.classList.contains('show')) {
        searchbar.querySelector('input').focus()
    }
    document.addEventListener('click', e => {
        if (!searchbar.contains(e.target)) {
            searchbar.classList.remove('show')
        }
    })
})




function initLoginForm() {
    console.log('NEED TO UPDATE THE PATH FOR IMAGE ELEMENT!!')
    const form = document.querySelector('.login_form')
    const bg = document.querySelector('.blur_overlay.login')

    document.querySelectorAll('.open_login_form_btn')
        .forEach(item => item.addEventListener('click', e => {
            form.classList.add('active')
            bg.classList.add('active')
            document.querySelector('.signup_form').classList.remove('active')
            document.querySelector('.blur_overlay.signup').classList.remove('active')
        }))

    function handleClose() {
        form.classList.remove('active')
        bg.classList.remove('active')
    }

    document.querySelector('.login_form_close_icon').addEventListener('click', handleClose)

    bg.addEventListener('click', handleClose)

}

function initSignUpForm() {
    console.log('NEED TO UPDATE THE PATH FOR IMAGE ELEMENT!!')
    const form = document.querySelector('.signup_form')
    const bg = document.querySelector('.blur_overlay.signup')


    document.querySelector('.open_signup_form_btn').addEventListener('click', e => {
        form.classList.add('active')
        bg.classList.add('active')
        document.querySelector('.login_form').classList.remove('active')
        document.querySelector('.blur_overlay.login').classList.remove('active')
    })

    function handleClose() {
        form.classList.remove('active')
        bg.classList.remove('active')
    }

    document.querySelector('.signup_form_close_icon').addEventListener('click', handleClose)
    bg.addEventListener('click', handleClose)

}

function initCartPreviewer() {
    const bg = document.querySelector('.blur_overlay.cart')
    document.querySelector('.open_cart_button')
        .addEventListener('click', e => {
            document.querySelector('.cart_previewer').classList.add('active')
            bg.classList.add('active')
        })

    function handleClose() {
        document.querySelector('.cart_previewer').classList.remove('active')
        bg.classList.remove('active')
    }
    bg.addEventListener('click', handleClose)
}

initCartPreviewer()
initLoginForm()
initSignUpForm()


