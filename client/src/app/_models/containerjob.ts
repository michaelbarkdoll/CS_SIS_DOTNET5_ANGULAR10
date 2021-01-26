
export interface ContainerJob {
    id: number;
    containerId: string;
    image: string;
    command: string;
    internalPort: number;
    externalPort: number;
    jobOwner: string;
    containerHost: string;
    containerStatus: string;
}
