class AuthService {
    constructor() {
        this.allowedOrigins = [];
        this.providers = {};
        this.logoutEndpoint = null;
        this.appRedirect = () => null;
        this.authenticatedCallbacks = [];
        this.signedOutCallbacks = [];
        window.addEventListener('message', (e) => this.onMessage(e), false);
    }
    get identity() {
        return ViWork.getIdentity();
    }
    set identity(i) {
        ViWork.setIdentity(i);
    }
    get isConnected() {
        return this.identity != null;
    }
    get accessToken() {
        const identity = this.identity;
        return identity ? identity.bearer.access_token : null;
    }
    get email() {
        const identity = this.identity;
        return identity ? identity.email : null;
    }
    hisEmail() {
        const identity = this.identity;
        return (identity.email);
    }
    get boundProviders() {
        const identity = this.identity;
        return identity ? identity.boundProviders : [];
    }
    isBoundToProvider(expectedProviders) {
        let isBound = false;
        for (const p of expectedProviders) {
            if (this.boundProviders.indexOf(p) > -1) {
                isBound = true;
            }
        }
        return isBound;
    }
    onMessage(e) {
        if (!e.origin || this.allowedOrigins.indexOf(e.origin) < 0 || e.data === '') {
            return;
        }
        const data = typeof e.data === 'string' ? JSON.parse(e.data) : e.data;
        if (data.type === 'authenticated') {
            this.onAuthenticated(data.payload);
        }
        else if (data.type === 'signedOut') {
            this.onSignedOut();
        }
    }
    login(selectedProvider) {
        const provider = this.providers[selectedProvider];
        const popup = window.open(provider.endpoint, 'Connexion', 'menubar=no, status=no, scrollbars=no, menubar=no, width=700, height=700');
    }
    registerAuthenticatedCallback(cb) {
        this.authenticatedCallbacks.push(cb);
    }
    removeAuthenticatedCallback(cb) {
        this.authenticatedCallbacks.splice(this.authenticatedCallbacks.indexOf(cb), 1);
    }
    onAuthenticated(i) {
        this.identity = i;
        for (const cb of this.authenticatedCallbacks) {
            cb();
        }
    }
    logout() {
        const popup = window.open(this.logoutEndpoint, 'Déconnexion', 'menubar=no, status=no, scrollbars=no, menubar=no, width=700, height=600');
    }
    registerSignedOutCallback(cb) {
        this.signedOutCallbacks.push(cb);
    }
    removeSignedOutCallback(cb) {
        this.signedOutCallbacks.splice(this.signedOutCallbacks.indexOf(cb), 1);
    }
    onSignedOut() {
        this.identity = null;
        for (const cb of this.signedOutCallbacks) {
            cb();
        }
    }
    async getToken() {
        let host;
        const result = await fetch(host + '/api/token', {
            credentials: 'include',
            method: 'GET',
            mode: 'cors',
            headers: {
                'Content-Type': 'application/json'
            },
        });
        if (result.ok) {
            const token = await result.json();
            if (token.success)
                return token;
        }
        return null;
    }
    async refreshToken() {
        this.identity = await this.getToken();
    }
    async init() {
        const token = await this.getToken();
        if (token !== null)
            this.onAuthenticated(token);
    }
}
export default new AuthService();
//# sourceMappingURL=AuthService.js.map