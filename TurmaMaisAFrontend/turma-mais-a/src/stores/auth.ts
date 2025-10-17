import { defineStore } from 'pinia';
import axios, { isAxiosError } from 'axios';
import router from '@/router'; // Importe o router para redirecionamento
import type { AuthState, LoginResult } from '@/types/auth';

function getInitialState(): AuthState {
  const token = localStorage.getItem('token') ?? "";
  const tokenExpiration = localStorage.getItem('tokenExpiration') ?? "";
  const organizationName = localStorage.getItem('organizationName') ?? "";
  const userFullname = localStorage.getItem('userFullname') ?? "";
  return { token, tokenExpiration, organizationName, userFullname };
}

export const useAuthStore = defineStore('auth', {
  state: (): AuthState => getInitialState(),

  getters: {
    isAuthenticated: (state): boolean => !!state.token
  },

  actions: {
    async login(credentials: { username: string; password: string }): Promise<LoginResult> {
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
        await router.push('/');
        return { success: true }
      } catch (error) {
        console.error('Falha no login:', error);
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
  },
});