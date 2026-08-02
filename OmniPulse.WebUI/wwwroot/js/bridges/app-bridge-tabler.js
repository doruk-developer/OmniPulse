/**
 * =========================================================================================
 * META-FRAMEWORK : THE LEVIATHAN TABLER JS BRIDGE (v2.0 - SUPREME SCOPED EDITION)
 * ARCHITECTURE: Global Scope Protection, Mutation Sentinel & Autonomous Hydration
 * DESCRIPTION: Initializes and isolates Tabler UI components strictly within .tabler-scope
 * =========================================================================================
 */

(function (window, document) {
    'use strict';

    // 🛡️ ÇEKİRDEK KONTROLÜ: Eğer Utils yüklü değilse, sahte bir telemetri objesi yarat (Hata yutucu)
    const Telemetry = window.ZenUI || {
        sysLog: (msg, type) => console.log(`[APP_TELEMETRY_${type.toUpperCase()}] ${msg}`)
    };

    /**
     * 🧠 TABLER BRIDGE CORE KERNEL
     * Tüm Tabler operasyonlarını hapseden küresel (fakat izole) nesne.
     */
    window.AppTablerBridge = {

        Version: "2.0.0-Leviathan",
        IsArmed: false,
        Observer: null,

        // Sistem Ayarları
        Config: {
            ScopeClass: '.tabler-scope',
            ShieldAttributes: true,     // data-bs-* özelliklerini data-tblr-* olarak otonom değiştirir
            AutoHydrate: true,          // Yeni eklenen DOM elementlerini otonom canlandırır
            DebugMode: false
        },

        /**
         * 🚀 1. SİSTEM ATEŞLEMESİ (Bootstrapper)
         * Sistem yüklendiğinde köprüyü ayağa kaldırır ve Sentinel'i (Gözcü) başlatır.
         */
        arm: function () {
            if (this.IsArmed) return;

            try {
                // Tabler kütüphanesinin fiziksel varlığını doğrula
                if (typeof window.tabler === 'undefined') {
                    Telemetry.sysLog("[TABLER_BRIDGE_FAULT]: tabler.js core is missing in the global scope.", "danger");
                    return;
                }

                // Mevcut DOM üzerindeki Tabler alanlarını canlandır
                this.scanAndHydrate(document);

                // Gelecekteki otonom enjeksiyonlar (AJAX/SignalR) için Kuantum Gözcüsünü başlat
                if (this.Config.AutoHydrate) {
                    this.deployMutationSentinel();
                }

                this.IsArmed = true;
                Telemetry.sysLog("[TABLER_BRIDGE]: Leviathan JS Shield armed & operational. Zero-Collision mode active.", "info");

            } catch (error) {
                Telemetry.sysLog(`[TABLER_BRIDGE_CRASH]: Kernel panic during arming sequence. ${error.message}`, "danger");
            }
        },

        /**
         * 🛡️ 2. SANITIZATION SHIELD (Attribute Çevirici)
         * Yapay Zeka HTML üretirken "data-bs-toggle" üretirse, bunu AdminLTE de dinlediği için sistem çökebilir/çakışabilir.
         * Bu metod DOM'daki tüm "data-bs-" kısımlarını "data-tblr-" yaparak Tabler'a özel hale getirir ve AdminLTE'den gizler.
         */
        sanitizeAttributes: function (container) {
            if (!this.Config.ShieldAttributes) return;

            // Kapsül içindeki tüm elementleri tara (Kendisi dahil)
            const elements = container.querySelectorAll ? container.querySelectorAll('*') : [];
            const targets = [container, ...Array.from(elements)];

            let mutationCount = 0;

            targets.forEach(el => {
                if (el && el.attributes) {
                    Array.from(el.attributes).forEach(attr => {
                        // Sadece data-bs- ile başlayan özellikleri yakala
                        if (attr.name.startsWith('data-bs-')) {
                            const newAttrName = attr.name.replace('data-bs-', 'data-tblr-');
                            el.setAttribute(newAttrName, attr.value);
                            el.removeAttribute(attr.name);
                            mutationCount++;
                        }
                    });
                }
            });

            if (this.Config.DebugMode && mutationCount > 0) {
                console.log(`[TABLER_SHIELD]: Sanitized ${mutationCount} potentially colliding 'data-bs-*' attributes to 'data-tblr-*'.`);
            }
        },

        /**
         * 💧 3. HYDRATOR ENGINE (Canlandırma Motoru)
         * Bootstrap 5 / Tabler bileşenleri (Tooltip, Popover vb.) Vanilla JS olduğu için manuel tetikleme ister.
         * Bu motor, sadece .tabler-scope içini tarayıp bileşenleri hayata döndürür.
         */
        hydrate: function (scopeElement) {
            if (!scopeElement || !scopeElement.querySelectorAll) return;

            try {
                // 3.1. Önce olası sızıntıları temizle (Sanitization)
                this.sanitizeAttributes(scopeElement);

                let hydratedCount = 0;
                const tablerApi = window.tabler;

                // --- A. TOOLTIPS (İpuçları) ---
                if (tablerApi.Tooltip) {
                    const tooltips = scopeElement.querySelectorAll('[data-tblr-toggle="tooltip"]');
                    tooltips.forEach(el => {
                        if (!tablerApi.Tooltip.getInstance(el)) {
                            new tablerApi.Tooltip(el, {
                                delay: { show: 50, hide: 50 },
                                html: el.getAttribute('data-tblr-html') === 'true'
                            });
                            hydratedCount++;
                        }
                    });
                }

                // --- B. POPOVERS (Baloncuklar) ---
                if (tablerApi.Popover) {
                    const popovers = scopeElement.querySelectorAll('[data-tblr-toggle="popover"]');
                    popovers.forEach(el => {
                        if (!tablerApi.Popover.getInstance(el)) {
                            new tablerApi.Popover(el, {
                                delay: { show: 50, hide: 50 },
                                html: el.getAttribute('data-tblr-html') === 'true'
                            });
                            hydratedCount++;
                        }
                    });
                }

                // --- C. CUSTOM SWITCHES (Özel Şalterler) ---
                const switches = scopeElement.querySelectorAll('[data-tblr-toggle="switch-icon"]');
                switches.forEach(el => {
                    // Mükerrer event atanmasını engellemek için custom data-flag kullanıyoruz
                    if (!el.hasAttribute('data-app-hydrated')) {
                        el.addEventListener('click', (e) => {
                            e.stopPropagation(); // AdminLTE'ye sızmasını engelle
                            el.classList.toggle('active');
                        });
                        el.setAttribute('data-app-hydrated', 'true');
                        hydratedCount++;
                    }
                });

                // --- D. THIRD-PARTY LIBS (ApexCharts, TomSelect vb.) ---
                // Burası gelecekte Tabler'ın vendor kütüphanelerini sarmalamak için rezerve edilmiştir.
                this.hydrateThirdParty(scopeElement);

                if (this.Config.DebugMode && hydratedCount > 0) {
                    console.log(`[TABLER_HYDRATOR]: Successfully initialized ${hydratedCount} encapsulated components.`);
                }

            } catch (error) {
                Telemetry.sysLog(`[TABLER_HYDRATION_FAULT]: Error during scope rendering. ${error.message}`, "warning");
            }
        },

        /**
         * 🎯 4. SCANNER (Tarayıcı)
         * Verilen kök dizindeki tüm .tabler-scope kapsüllerini bulur ve Hydrator'a paslar.
         */
        scanAndHydrate: function (rootElement) {
            // Eğer rootElement bizzat tabler-scope ise doğrudan canlandır
            if (rootElement.classList && rootElement.classList.contains(this.Config.ScopeClass.replace('.', ''))) {
                this.hydrate(rootElement);
            }

            // İçindeki tüm tabler-scope kapsüllerini bul ve canlandır
            const scopes = rootElement.querySelectorAll ? rootElement.querySelectorAll(this.Config.ScopeClass) : [];
            scopes.forEach(scope => this.hydrate(scope));
        },

        /**
         * 👁️ 5. KUANTUM GÖZCÜSÜ (Mutation Sentinel)
         * Sistem, C# Weaver'ı veya SignalR arka planda DOM'a yeni bir Tabler kodu basarsa,
         * sistemin yeniden yüklenmesine (F5) gerek kalmadan anında fark eder ve canlandırır.
         */
        deployMutationSentinel: function () {
            if (this.Observer) return;

            const self = this;
            const targetNode = document.body;
            const config = { childList: true, subtree: true };

            const callback = function (mutationsList, observer) {
                for (const mutation of mutationsList) {
                    if (mutation.type === 'childList') {
                        mutation.addedNodes.forEach(node => {
                            // Eğer eklenen node geçerli bir HTML elementi ise
                            if (node.nodeType === Node.ELEMENT_NODE) {
                                // Node'un kendisi veya içindeki çocukları tabler-scope mu?
                                self.scanAndHydrate(node);
                            }
                        });
                    }
                }
            };

            this.Observer = new MutationObserver(callback);
            this.Observer.observe(targetNode, config);

            if (this.Config.DebugMode) {
                console.log("[TABLER_SENTINEL]: DOM Mutation Observer deployed and tracking for dynamic scope injections.");
            }
        },

        /**
         * 🔌 6. 3RD PARTY ADAPTERS (Genişleme Portları)
         * İleride ApexCharts, TomSelect gibi kütüphaneleri izole şekilde tetiklemek için ayrılmış port.
         */
        hydrateThirdParty: function (scopeElement) {
            // Gelecek versiyonlarda Tabler'ın vendor kütüphaneleri eklendiğinde
            // bu fonksiyon içinden otonom olarak (Sadece scopeElement içinde kalarak) tetiklenecek.
            // Örn: const charts = scopeElement.querySelectorAll('[data-chart-id]'); ...
        },

        /**
         * 🛑 7. ACİL DURUM KAPATMASI (Kill Switch)
         */
        disarm: function () {
            if (this.Observer) {
                this.Observer.disconnect();
                this.Observer = null;
            }
            this.IsArmed = false;
            Telemetry.sysLog("[TABLER_BRIDGE]: Leviathan Shield disarmed.", "warning");
        }
    };

    // =====================================================================================
    // 🚀 AUTO-IGNITION PROTOCOL (Belge Yüklendiğinde Köprüyü Otonom Başlat)
    // =====================================================================================
    window.addEventListener('DOMContentLoaded', function () {
        // Sistem'in ana yükleme sırasını bozmamak için ufak bir asenkron gecikme (100ms)
        setTimeout(() => {
            window.AppTablerBridge.arm();
        }, 100);
    });

})(window, document);