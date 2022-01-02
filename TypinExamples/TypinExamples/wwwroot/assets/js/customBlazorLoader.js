var __customBlazorLoader = {};

__customBlazorLoader.barId = 'blazor-boot-bar';
__customBlazorLoader.counter = 0;
__customBlazorLoader.total = -1;

__customBlazorLoader.callback = (config, keys, name, response) => {
    const c = __customBlazorLoader;

    if (name.endsWith(".dll") || name.endsWith(".pdb")) {
        const p = document.getElementById(c.barId);
        p.value = ++c.counter;

        if (c.total <= 0) {
            const res = config.bootConfig.resources;
            const atl = Object.keys(res.assembly).length;
            const ptl = res.pdb ? Object.keys(res.pdb).length : 0;
            p.max = c.total = atl + ptl;

            console.log(`[__customBlazorLoader] ${atl} .dll & ${ptl} .pdb to load`);
        }

        //if (c.total == c.counter) {
        //    p.className = "is-hidden";
        //}
    }

    const cp = Math.ceil((c.counter * 100.0) / c.total);
    console.debug(`[__customBlazorLoader] ${c.counter}/${c.total} (${cp}%) [${name.substr(name.lastIndexOf('.') + 1)}] ${name}`);
}

window.__customBlazorLoader = __customBlazorLoader;

fetch("_framework/blazor.webassembly.js")
    .then(response => response.text())
    .then(t => {
        const p = t.replace(
            "return r.loadResource(o,t(o),e[o],n)",
            "var p = r.loadResource(o,t(o),e[o],n); p.response.then((x) => { const z = window.__customBlazorLoader.callback; if (typeof z === 'function') { z(r, Object.keys(e), o, x);}}); return p;"
        );

        const s = document.createElement('script');
        s.textContent = p;
        document.body.appendChild(s);
    });