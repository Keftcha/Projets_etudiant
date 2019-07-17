import JSZip from "jszip"

export function make_archive(elements: Map<any, any>, archive = new JSZip()) {
    for (let elem in elements) {archive.file(elem + ".pdf", elements.get(elem))}
    return archive
}