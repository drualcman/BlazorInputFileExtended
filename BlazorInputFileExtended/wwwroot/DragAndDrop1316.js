/**** 
 * DrUalcman 17 Jun 2021
 * Author: Sergi Ortiz Gomez
 * Version 1.2.7 BlazorInputFileExtended
 * License: Apache License, Version 2.0 
 *          SPDX short identifier: Apache-2.0
 * Update:  05 September 2021 Version 1.2.16
 *          Update how to manage the object reference from Blazor and to Blazor
 *          Add paste function
 ****/
/** Export DragAndDrop to load the file dynamically */
export function DragAndDrop(dropZone, inputContainer) {
    const inputFile = inputContainer.querySelector('input[type="file"]');

    function onDropHandler(ev) {
        // Prevent default behavior (Prevent file from being opened)
        ev.preventDefault();
        // send the files to the input file
        inputFile.files = ev.dataTransfer.files;
        // Generate the change event to notify to Blazor have some change.
        try {
            let event = new Event('change', { bubbles: true }); // Create the event.
            inputFile.dispatchEvent(event);                     // Dispatch the event.
        } catch (e) {
            console.warn(e);
        }
        // clean the drag and drop data
        RemoveDragData(ev)
    }

    function onPaste(ev) {
        inputFile.files = ev.clipboardData.files;
        let event = new Event('change', { bubbles: true });
        inputFile.dispatchEvent(event);
    }

    /*** Setup the events */
    RemoveEvents();
    dropZone.addEventListener('dragover', onDragOverHandler, false);
    dropZone.addEventListener('drop', onDropHandler, false);
    document.body.addEventListener('paste', onPaste, false);

    function onDragOverHandler(ev) {
        // Prevent default behavior (Prevent file from being opened)
        ev.preventDefault();
        ev.stopPropagation();
    }

    function RemoveDragData(ev) {
        if (ev.dataTransfer.items) {
            // Use DataTransferItemList interface to remove the drag data
            ev.dataTransfer.items.clear();
        } else {
            // Use DataTransfer interface to remove the drag data
            ev.dataTransfer.clearData();
        }
    }

    function RemoveEvents() {
        try {
            dropZone.removeEventListener('dragover', onDragOverHandler, false);
            dropZone.removeEventListener('drop', onDropHandler, false);
            document.body.removeEventListener('paste', onPaste, false);
        } catch (e) {
            console.warn('DragAndDrop: RemoveEvents', e);
        }
    }

    return {
        Dispose: () => {
            RemoveEvents();
        }
    }
}

