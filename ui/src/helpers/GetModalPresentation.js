class ModalPresentation {
    constructor(modalOk, modalCancel) {
        this.message = "";
        this.errors = [];
        this.visible = false;
        this.modalOk = modalOk;
        this.modalCancel = modalCancel;
    };
};

const GetModalPresentation = (modalOk, modalCancel) => {
    let presentation = new ModalPresentation(modalOk, modalCancel);
    return presentation;
}

export default GetModalPresentation;