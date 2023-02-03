function ValidateInput() {
    if (document.getElementById("uploadBox").value == "") {
        Swal.fire({
            icon: 'error',
            title: 'Oops...',
            text: 'Please upload an image!'
        })
        return false;
    }

    return true;
}        