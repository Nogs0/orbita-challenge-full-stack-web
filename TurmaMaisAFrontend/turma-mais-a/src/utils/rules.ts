export const rules = {
    required: (value: string) => !!value || 'Este campo é obrigatório.',
    minLength: (limit: number) => {
        return (v: string) => (v && v.length >= limit) || `Mínimo de ${limit} caracteres permitidos.`;
    },
    maxLength: (limit: number) => {
        return (v: string) => (v && v.length <= limit) || `Máximo de ${limit} caracteres permitidos.`;
    },
    passwordStrength: (v: string) => {
        const passwordPattern = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=\[\]{};':"\\|,.<>\/?]).{8,}$/;

        return passwordPattern.test(v) ||
            'A senha deve conter no mínimo 8 caracteres, incluindo ao menos uma letra maiúscula, uma minúscula, um número e um símbolo especial.';
    },
    passwordMatch: (originalPassword: string) => (v: string) =>
        v === originalPassword || 'As senhas não conferem.',
    email: (v: string) => /.+@.+\..+/.test(v) || 'E-mail deve ser válido.',
};