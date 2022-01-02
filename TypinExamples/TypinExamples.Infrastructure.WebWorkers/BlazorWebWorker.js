﻿window.BlazorWebWorker = function () {

    const workers = {};

    const disposeWorker = function (workerId) {
        const worker = workers[workerId];
        if (worker && worker.terminate) {
            worker.terminate();
            delete workers[workerId];
        }
    };

    const workerDef = function () {
        const initConf = JSON.parse('$initConf$');
        const onReady = () => {

            if (!initConf.InitEndpoint || !initConf.MessageEndpoint) {
                console.error(`[WASM-WORKER ${initConf.WorkerId}] Init failed for call ${initConf.InitCallId}: either InitEndpoint ${initConf.InitEndpoint} or MessageEndpoint ${initConf.MessageEndpoint} is invalid.`);
                return;
            }

            try {
                const messageHandler = Module.mono_bind_static_method(initConf.MessageEndpoint);

                // Future messages goes directly to the message handler
                self.onmessage = msg => {
                    messageHandler(msg.data);
                };
            } catch (e) {
                console.error(`[WASM-WORKER ${initConf.WorkerId}] Init failed for call ${initConf.InitCallId}: Message endpoint init ${initConf.MessageEndpoint} thrown an error.`, e);
                throw e;
            }

            try {
                Module.mono_call_static_method(initConf.InitEndpoint, [initConf.WorkerId, initConf.InitCallId, initConf.StartupType]);
            } catch (e) {
                console.error(`[WASM-WORKER ${initConf.WorkerId}] Init failed for call ${initConf.InitCallId}: Init method ${initConf.InitEndpoint} thrown an error.`, e);
                throw e;
            }
        };

        const onError = (err) => {
            console.error(err);
        };

        function asyncLoad(url, reponseType) {
            return new Promise((resolve, reject) => {
                const xhr = new XMLHttpRequest();
                const arrayBufferType = 'arraybuffer';
                xhr.open('GET', url, /* async: */ true);
                xhr.responseType = reponseType || arrayBufferType;
                xhr.onload = function xhr_onload() {
                    if (xhr.status == 200 || xhr.status == 0 && xhr.response) {
                        if (this.responseType === arrayBufferType) {
                            const asm = new Uint8Array(xhr.response);
                            resolve(asm);
                        } else {
                            resolve(xhr.response);
                        }
                    } else {
                        reject(xhr);
                    }
                };
                xhr.onerror = reject;
                xhr.send(undefined);
            });
        }

        var config = {};
        var Module = {};

        const wasmBinaryFile = `${initConf.appRoot}/${initConf.wasmRoot}/dotnet.wasm`;
        const suppressMessages = ['DEBUGGING ENABLED'];
        const appBinDirName = 'appBinDir';

        Module.print = line => (suppressMessages.indexOf(line) < 0 && console.log(`[WASM-WORKER ${initConf.WorkerId}] ${line}`));

        Module.printErr = line => {
            console.error(`[WASM-WORKER ${initConf.WorkerId}] Error: ${line}`);
            showErrorNotification();
        };
        Module.preRun = [];
        Module.postRun = [];
        Module.preloadPlugins = [];

        Module.locateFile = fileName => {
            switch (fileName) {
                case 'dotnet.wasm': return wasmBinaryFile;
                default: return fileName;
            }
        };

        Module.preRun.push(() => {
            const mono_wasm_add_assembly = Module.cwrap('mono_wasm_add_assembly', null, [
                'string',
                'number',
                'number',
            ]);

            mono_string_get_utf8 = Module.cwrap('mono_wasm_string_get_utf8', 'number', ['number']);

            MONO.loaded_files = [];
            const baseUrl = `${initConf.appRoot}/${initConf.deploy_prefix}`;
            initConf.DependentAssemblyFilenames.forEach(url => {

                const runDependencyId = `blazor:${url}`;
                addRunDependency(runDependencyId);

                asyncLoad(baseUrl + '/' + url).then(
                    data => {
                        const heapAddress = Module._malloc(data.length);
                        const heapMemory = new Uint8Array(Module.HEAPU8.buffer, heapAddress, data.length);
                        heapMemory.set(data);
                        mono_wasm_add_assembly(url, heapAddress, data.length);
                        MONO.loaded_files.push(url);
                        removeRunDependency(runDependencyId);
                    },
                    errorInfo => {
                        const isPdb404 = errorInfo instanceof XMLHttpRequest
                            && errorInfo.status === 404
                            && url.match(/\.pdb$/);
                        if (!isPdb404) {
                            onError(errorInfo);
                        }
                        removeRunDependency(runDependencyId);
                    }
                );
            });
        });

        Module.postRun.push(() => {
            MONO.mono_wasm_setenv("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");
            const load_runtime = Module.cwrap('mono_wasm_load_runtime', null, ['string', 'number']);
            load_runtime(appBinDirName, 0);
            MONO.mono_wasm_runtime_is_ready = true;
            onReady();
        });

        config.file_list = [];

        global = globalThis;
        self.Module = Module;

        //TODO: This call could/should be session cached. But will the built-in blazor fetch service worker override
        // (PWA et al) do this already if configured ?
        asyncLoad(`${initConf.appRoot}/${initConf.blazorBoot}`, 'json')
            .then(blazorboot => {
                let dotnetjsfilename = '';
                const runttimeSection = blazorboot.resources.runtime;
                for (let p in runttimeSection) {
                    if (Object.prototype.hasOwnProperty.call(runttimeSection, p) && p.endsWith('.js')) {
                        dotnetjsfilename = p;
                    }
                }

                if (dotnetjsfilename === '') {
                    throw `[WASM-WORKER ${initConf.WorkerId}] Unable to locate dotnetjs file in blazor boot config.`;
                }

                self.importScripts(`${initConf.appRoot}/${initConf.wasmRoot}/${dotnetjsfilename}`);

            }, errorInfo => onError(errorInfo));
    };

    const inlineWorker = `self.onmessage = ${workerDef}()`;

    const initWorker = function (id, callbackInstance, initOptions) {
        let appRoot = (document.getElementsByTagName('base')[0] || { href: window.location.origin }).href || "";
        if (appRoot.endsWith("/")) {
            appRoot = appRoot.substr(0, appRoot.length - 1);
        }

        if (initOptions.debug)
            console.log(`[WASM-WORKER ${initConf.WorkerId}] Worker with id ${initOptions.WorkerId} is in debug mode.`);

        const initConf = {
            appRoot: appRoot,
            DependentAssemblyFilenames: initOptions.dependentAssemblyFilenames,
            deploy_prefix: "_framework",
            WorkerId: id,
            MessageEndpoint: initOptions.messageEndpoint,
            InitEndpoint: initOptions.initEndpoint,
            InitCallId: initOptions.initCallId,
            StartupType: initOptions.startupType,
            wasmRoot: "_framework",
            blazorBoot: "_framework/blazor.boot.json",
            debug: initOptions.debug,
        };

        // Initialize worker
        const renderedConfig = JSON.stringify(initConf).replace('$appRoot$', appRoot);
        const renderedInlineWorker = inlineWorker.replace('$initConf$', renderedConfig);
        window.URL = window.URL || window.webkitURL;
        const blob = new Blob([renderedInlineWorker], { type: 'application/javascript' });
        const worker = new Worker(URL.createObjectURL(blob), { name: `[WASM-WORKER ${initConf.WorkerId}]` });
        workers[id] = worker;

        worker.onmessage = function (ev) {
            if (initOptions.debug) {
                console.debug(`[WASM-WORKER ${initConf.WorkerId}] worker->blazor`, initOptions.callbackMethod, ev.data);
            }

            callbackInstance.invokeMethod(initOptions.callbackMethod, ev.data);
        };
    };

    const postMessage = function (workerId, message) {
        workers[workerId].postMessage(message);
    };

    return {
        disposeWorker,
        initWorker,
        postMessage
    };
}();