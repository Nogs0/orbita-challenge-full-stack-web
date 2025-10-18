import { ref, readonly } from 'vue';

const isVisible = ref<boolean>(false);
const text = ref<string>('');
const color = ref<'success' | 'error' | 'info' | 'warning'>('success');

export function useSnackbar() {
  // Função para mostrar o snackbar
  const showSnackbar = (newText: string, newColor: 'success' | 'error' | 'info' | 'warning' = 'success') => {
    text.value = newText;
    color.value = newColor;
    isVisible.value = true;
  };

  return {
    isVisible: isVisible,
    text: readonly(text),
    color: readonly(color),
    showSnackbar,
  };
}