export function formatarCPF(value: string | null | undefined): string {
  if (!value) {
    return '';
  }
  const cpfApenasNumeros = value.replace(/\D/g, '');

  const cpfLimitado = cpfApenasNumeros.slice(0, 11);

  let cpfFormatado = cpfLimitado;
  cpfFormatado = cpfFormatado.replace(/(\d{3})(\d)/, '$1.$2');
  cpfFormatado = cpfFormatado.replace(/(\d{3})\.(\d{3})(\d)/, '$1.$2.$3');
  cpfFormatado = cpfFormatado.replace(/(\d{3})\.(\d{3})\.(\d{3})(\d{1,2})/, '$1.$2.$3-$4');

  return cpfFormatado;
}