var anhItemS = document.getElementsByClassName('image_item')
var anhDuocHienThi = document.getElementsByClassName('showed_image')[0]
let listSrcImage = []
let indexImage = 0
for (let i = 0; i < anhItemS.length; i++) {
    listSrcImage.push(anhItemS[i].getAttribute('src'))
}

anhDuocHienThi.src = listSrcImage[0]


function chonAnh(k) {
    anhItemS[indexImage].classList.remove('border_image')
    indexImage = k
    anhItemS[indexImage].classList.add('border_image')
    anhDuocHienThi.src = listSrcImage[indexImage]
}
for (let i = 0; i < anhItemS.length; i++) {
    anhItemS[i].addEventListener('click', function () {
        chonAnh(i)
    })
}