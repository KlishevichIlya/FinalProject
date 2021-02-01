//import { Modal } from "../lib/bootstrap/dist/js/bootstrap.bundle";

document.querySelectorAll(".drop-zone__input").forEach((inputElement) => {
  const dropZoneElement = inputElement.closest(".drop-zone");

  dropZoneElement.addEventListener("click", (e) => {
    inputElement.click();
  });

  inputElement.addEventListener("change", (e) => {
    if (inputElement.files.length) {
      updateThumbnail(dropZoneElement, inputElement.files[0]);
    }
  });

  dropZoneElement.addEventListener("dragover", (e) => {
    e.preventDefault();
    dropZoneElement.classList.add("drop-zone--over");
  });

  ["dragleave", "dragend"].forEach((type) => {
    dropZoneElement.addEventListener(type, (e) => {
      dropZoneElement.classList.remove("drop-zone--over");
    });
  });

  dropZoneElement.addEventListener("drop", (e) => {
    e.preventDefault();

    if (e.dataTransfer.files.length) {
      inputElement.files = e.dataTransfer.files;
      updateThumbnail(dropZoneElement, e.dataTransfer.files[0]);
    }

    dropZoneElement.classList.remove("drop-zone--over");
  });
});

/**
 * Updates the thumbnail on a drop zone element.
 *
 * @param {HTMLElement} dropZoneElement
 * @param {File} file
 */
function updateThumbnail(dropZoneElement, file) {
  let thumbnailElement = dropZoneElement.querySelector(".drop-zone__thumb");
  
  if (dropZoneElement.querySelector(".drop-zone__prompt")) {
    dropZoneElement.querySelector(".drop-zone__prompt").remove();
    }
    
  if (!thumbnailElement) {
    thumbnailElement = document.createElement("div");
    thumbnailElement.classList.add("drop-zone__thumb");
    dropZoneElement.appendChild(thumbnailElement);
  }

  thumbnailElement.dataset.label = file.name;
  
  if (file.type.startsWith("image/")) {
    const reader = new FileReader();

    reader.readAsDataURL(file);
    reader.onload = () => {
      thumbnailElement.style.backgroundImage = `url('${reader.result}')`;
    };
  } else {
    thumbnailElement.style.backgroundImage = null;
  }
}


//let textarea = document.getElementById("textarea");
//textarea.onchange = function () {
    
//    console.log(parseMarkdown(textarea.value));
//    var it = parseMarkdown(textarea.value);
    
//    textarea.value = it;
   
//}


////mardown parser 
//function parseMarkdown(markdownText) {
//    const htmlText = markdownText
//        .replace(/^### (.*$)/gim, '<h3>$1</h3>')
//        .replace(/^## (.*$)/gim, '<h2>$1</h2>')
//        .replace(/^# (.*$)/gim, '<h1>$1</h1>')
//        .replace(/^\> (.*$)/gim, '<blockquote>$1</blockquote>')
//        .replace(/\*\*(.*)\*\*/gim, '<b>$1</b>')
//        .replace(/\*(.*)\*/gim, '<i>$1</i>')
//        .replace(/!\[(.*?)\]\((.*?)\)/gim, "<img alt='$1' src='$2' />")
//        .replace(/\[(.*?)\]\((.*?)\)/gim, "<a href='$2'>$1</a>")
//        .replace(/\n$/gim, '<br />')

//    return htmlText.trim()
//}

