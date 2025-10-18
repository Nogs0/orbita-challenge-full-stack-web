import { defineStore } from 'pinia';
import axios, { isAxiosError } from 'axios';
import router from '@/router'; // Importe o router para redirecionamento
import type { AuthState, LoginDto, AuthResultDto, RegisterDto } from '@/types/auth';
import { ca } from 'vuetify/locale';

function getInitialState(): AuthState {
  const token = localStorage.getItem('token') ?? "";
  const tokenExpiration = localStorage.getItem('tokenExpiration') ?? "";
  if (!token || !tokenExpiration) {
    return {
      token: null,
      tokenExpiration: null,
      organizationName: null,
      userFullname: null,
      isLoading: false,
      error: null,
    };
  }

  const expirationDate = new Date(tokenExpiration);
  const now = new Date();

  if (expirationDate > now) {
    const organizationName = localStorage.getItem('organizationName') ?? "";
    const userFullname = localStorage.getItem('userFullname') ?? "";
    const isLoading = false;
    const error = null;
    axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    return { token, tokenExpiration, organizationName, userFullname, isLoading, error };
  }
  else {
    localStorage.clear();
    return {
      token: "",
      tokenExpiration: "",
      organizationName: "",
      userFullname: "",
      isLoading: false,
      error: null
    }
  }
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => getInitialState(),
  getters: {
    isAuthenticated: (state): boolean => !!state.token
  },
  actions: {
    async login(credentials: LoginDto): Promise<AuthResultDto> {
      this.isLoading = true;
      this.error = null;
      try {
        const response = await axios.post<AuthState>('/Auth/login', credentials);

        // Atualiza o state
        this.token = response.data.token;
        this.tokenExpiration = response.data.tokenExpiration;
        this.organizationName = response.data.organizationName;
        this.userFullname = response.data.userFullname;

        // Persiste no localStorage
        localStorage.setItem('token', this.token ?? "");
        localStorage.setItem('tokenExpiration', this.tokenExpiration ?? "");
        localStorage.setItem('organizationName', this.organizationName ?? "");
        localStorage.setItem('userFullname', this.userFullname ?? "");

        // Adiciona o token aos headers padrão do Axios para futuras requisições
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`;

        // Redireciona para a área logada
        await router.push('/app');
        return { success: true }
      } catch (error) {
        console.error('Login failed:', error);
        this.token = null;
        this.tokenExpiration = null;
        this.organizationName = null;
        this.userFullname = null;
        this.logout(false);

        let message = 'Ocorreu um erro inesperado. Tente novamente.';

        if (isAxiosError(error) && error.response?.data?.errorMessage) {
          message = error.response.data.errorMessage;
        }
        return {
          success: false,
          message: message
        }
      }
      finally {
        this.isLoading = false;
      }
    },

    logout(redirect = true): void {
      this.token = null;
      this.tokenExpiration = null;
      this.organizationName = null;
      this.userFullname = null;

      localStorage.clear();

      delete axios.defaults.headers.common['Authorization'];

      if (redirect) {
        router.push('/login');
      }
    },
    async register(dto: RegisterDto): Promise<AuthResultDto> {
      try {
        const response = await axios.post<AuthState>('/Auth/register', dto);

        // Atualiza o state
        this.token = response.data.token;
        this.tokenExpiration = response.data.tokenExpiration;
        this.organizationName = response.data.organizationName;
        this.userFullname = response.data.userFullname;

        // Persiste no localStorage
        localStorage.setItem('token', this.token ?? "");
        localStorage.setItem('tokenExpiration', this.tokenExpiration ?? "");
        localStorage.setItem('organizationName', this.organizationName ?? "");
        localStorage.setItem('userFullname', this.userFullname ?? "");

        // Adiciona o token aos headers padrão do Axios para futuras requisições
        axios.defaults.headers.common['Authorization'] = `Bearer ${this.token}`;

        // Redireciona para a área logada
        await router.push('/app');
        return { success: true }
      } catch (error) {
        console.error('Register failed:', error);
        this.token = null;
        this.tokenExpiration = null;
        this.organizationName = null;
        this.userFullname = null;
        this.logout(false);

        let message = 'Ocorreu um erro inesperado. Tente novamente.';

        if (isAxiosError(error) && error.response?.data?.errorMessage) {
          message = error.response.data.errorMessage;
        }
        return {
          success: false,
          message: message
        }
      }
      finally {
        this.isLoading = false;
      }
    }
  },
});