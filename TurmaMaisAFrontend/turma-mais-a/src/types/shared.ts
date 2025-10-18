export interface PagedInputDto {
    pageNumber: number;
    pageSize: number;
    search?: string;
}

export interface PagedResultDto<TListDto> {
    items: TListDto[],
    totalCount: number
}