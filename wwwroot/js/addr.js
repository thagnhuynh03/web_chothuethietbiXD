const province_url = "https://api.npoint.io/ac646cb54b295b9555be";
const district_url = "https://api.npoint.io/34608ea16bebc5cffd42";
const ward_url = "https://api.npoint.io/dd278dc276e65c68cdf5";

var provinceList = [];
var districtList = [];
var wardList = [];

const onClickOption = (parentId, value, name) => {
  const select = document.getElementById(parentId);
  const parent = select.closest(".select-menu")
  const options = document.querySelectorAll(`[data-id="${parentId}"]`)


  options.forEach((o) => {
    o.classList.remove("selected");
  });

  parent.querySelector("span").innerHTML = name;
  parent.querySelector("span").setAttribute("data-value", value);

  document.querySelector(`div[value="${value}"]`).classList.add("selected");

  console.log(  document.querySelector(`div[value="${value}"]`))

  handleProvinceChange(parentId, value);

  parent.querySelector(".options-list").classList.remove("active");
  parent.querySelector(".fa-angle-down").classList.toggle("fa-angle-up");
};

const getAllDataForDropdown = async () => {
  [provinceList, districtList, wardList] = await Promise.all([
    $.getJSON(province_url),
    $.getJSON(district_url),
    $.getJSON(ward_url),
  ]);

  provinceList.forEach((element) => {
    $("#province")
      .closest(".select-menu")
      .children(".options-list")
      .append(
        `<div class="option" data-id="province" onclick="onClickOption('province',${element.Id}, '${element.Name}')" value="${element.Id}">${element.Name}</div>`
      );
  });
};

const getData = (data, id, placeholder, callBackGetData) => {
  const currentData = callBackGetData(data);

  $(`#${id}`).closest(".select-menu").children(".options-list").empty();
  $(`#${id}`).children("span").empty()
  $(`#${id}`).children("span").append(placeholder);

  document
    .querySelectorAll(".options-list")
    .forEach((item) => item.classList.remove("active"));

  currentData.forEach((element) => {
    $(`#${id}`).closest(".select-menu")
      .children(".options-list")
      .append(
        `<div class="option" value="${element.Id}" data-id="${id}" onclick="onClickOption('${id}',${element.Id}, '${element.Name}')">${element.Name}</div>`
      );
  });
};

const handleProvinceChange = (id, dataId) => {
  switch (id) {
    case "province": {
      Promise.all([
        getData(districtList, "district", "Quận/Huyện", function (data) {
          return data.filter((item) => item.ProvinceId === Number(dataId));
        }),

        getData(wardList, "ward", "Xã/Phường", function (data) {
          return [];
        }),
      ]);

      break;
    }
    case "district": {
      getData(wardList, "ward", "Xã/Phường", function (data) {
        return data.filter((item) => item.DistrictId === Number(dataId));
      });
      break;
    }
    default: {
      return;
    }
  }
};


function onClickSelect(element){

    document.querySelectorAll(".options-list").forEach(item=> item.classList.remove("active"))
    const parent = element.target.offsetParent;
    
    const listOptions = parent.querySelector(".options-list");

    listOptions.classList.toggle("active");
    parent.querySelector(".fa-angle-down").classList.toggle("fa-angle-up");
}

$(document).ready(function () {
  getAllDataForDropdown();

  $("#province").click(function (element) {
    onClickSelect(element)
  });
  $("#district").click(function (element) {
    onClickSelect(element)
  });
  $("#ward").click(function (element) {
    onClickSelect(element)
  });
});